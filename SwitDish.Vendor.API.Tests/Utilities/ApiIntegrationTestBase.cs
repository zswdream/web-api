using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NUnit.Framework;
using SwitDish.Vendor.Domain.Mappers;
using SwitDish.Vendor.Domain.Services;
using SwitDish.DataModel_OLD.Models;
using SwitDish.Vendor.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;

namespace SwitDish.Vendor.API.Tests.Utilities
{
    public class ApiIntegrationTestsBase : IntegrationTestBase, IDisposable
    {
        private static bool isAutoMapperInitialised;

        /// <summary>
        /// The legacy URL for this environment.
        /// </summary>
        private readonly string legacyAppUrl;

        /// <summary>
        /// Track whether Dispose has been called yet.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// The unity container.
        /// </summary>
        private UnityContainer unityContainer;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiIntegrationTestsBase()
        {
            this.legacyAppUrl = "http://localhost:51116";
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ApiIntegrationTestsBase"/> class.
        /// </summary>
        /// <remarks>
        /// This destructor will run only if the Dispose method does not get called.
        /// It gives your base class the opportunity to finalize.
        /// Do not provide destructor in types derived from this class.
        /// </remarks>
        ~ApiIntegrationTestsBase()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// The WebAPI server
        /// </summary>
        protected HttpServer Server
        {
            get; private set;
        }

        /// <summary>
        /// Configure a JSON formatter for the tests that suppresses null values
        /// </summary>
        /// <value>
        /// The json formatter.
        /// </value>
        protected JsonMediaTypeFormatter JsonFormatter { get; } = new JsonMediaTypeFormatter
        {
            SerializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }
        };

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Clean up this object with the Dispose method.
            this.Dispose(true);

            // then call GC.SuppressFinalize to take this object off the finalization queue
            // and prevent finalization code for this object from executing a second time.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// base set up for API integration tests.
        /// </summary>
        [SetUp]
        public void ApiIntegrationTestsBaseSetUp()
        {
            var httpConfig = new HttpConfiguration();


            this.UnityConfigRegisterComponents(httpConfig);

            Configure(httpConfig);

            // Initialise AutoMapper.
            this.InitialiseAutoMapper();

            // Improve error reporting?
            httpConfig.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            // Create the in-memory web server.
            this.Server = new HttpServer(httpConfig);
        }

        /// <summary>
        /// Gets the result from the HttpResponseMessage, checks it for errors and prints them,
        /// or returns the payload as a prettyprinted string
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns>the json payload form the http call</returns>
        protected static string GetCheckedResult(HttpResponseMessage response)
        {
            Assert.IsNotNull(response.Content, "Response is Null");

            string payload = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                Assert.Fail("Error from HttpResponseMessage: " + payload);
            }

            return payload.JsonPrettyPrint();
        }

        /// <summary>
        /// Gets the result from the HttpResponseMessage, checks it for errors and prints them,
        /// or returns the payload as a deserialised object
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns>the json payload form the http call</returns>
        protected static T GetCheckedResultObject<T>(HttpResponseMessage response)
        {
            Assert.IsNotNull(response.Content, "Response is Null");

            string payload = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                Assert.Fail("Error from HttpResponseMessage: " + payload);
            }

            return Json.Deserialise<T>(payload);
        }

        /// <summary>
        /// Creates a request that returns json.
        /// </summary>
        /// <param name="uriPath">The URI Path.</param>
        /// <param name="method">The method.</param>
        /// <returns></returns>
        protected HttpRequestMessage CreateRequest(string uriPath, HttpMethod method)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(this.legacyAppUrl + uriPath)
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Method = method;
            return request;
        }

        /// <summary>
        /// Creates a request that serialises the payload to json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uriPath">The URI Path.</param>
        /// <param name="method">The method.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        protected HttpRequestMessage CreateRequest<T>(string uriPath, HttpMethod method, T content)
            where T : class
        {
            HttpRequestMessage request = this.CreateRequest(uriPath, method);
            request.Content = new ObjectContent<T>(content, this.JsonFormatter);
            return request;
        }

        /// <summary>
        /// Release managed and unmanaged resources
        /// </summary>
        /// <param name="disposing">true during polite dispose, false during finalizer</param>
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing, dispose all managed and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    if (this.Server != null)
                    {
                        this.Server.Dispose();
                        this.Server = null;
                    }

                    if (this.unityContainer != null)
                    {
                        this.unityContainer.Dispose();
                        this.unityContainer = null;
                    }
                }

                // Call the appropriate methods to clean up unmanaged resources here.
                // this is the  where you will close and dispose all the IConnection object, IStream object
                // in fact every thing that the GC will not collect on it own have to explicitly disposed in this methods
                // If disposing is false, only the following code is executed.

                // Note disposing has been done.
                this.disposed = true;
            }
        }

        private void UnityConfigRegisterComponents(HttpConfiguration httpConfig)
        {
            this.unityContainer = new UnityContainer();

            this.RegisterTypes(this.unityContainer);

            // Register Unity as DI provider for Web API
            httpConfig.DependencyResolver = new UnityDependencyResolver(this.unityContainer);
        }

        private void RegisterTypes(IUnityContainer container)
        {
            // register services
            container.RegisterType<IAgentService, AgentService>(new HierarchicalLifetimeManager());
            container.RegisterType<SwitDishDatabaseContext, SwitDishDatabaseContext>();
            //container.RegisterType<IMapper, Mapper>(new HierarchicalLifetimeManager());
            //container.RegisterType<IConfigurationProvider, ConfigurationProvider>(new HierarchicalLifetimeManager());
        }

        /// <summary>
        /// Configures WebAPI by removing the xml formatter, forcing it to use json.
        /// </summary>
        /// <remarks>
        /// this is called by the website global.asax.cs as well as the hosted tests setup
        /// </remarks>
        /// <param name="config">The configuration.</param>
        private static void Configure(HttpConfiguration config)
        {
            // Enable Web API attribute routing
            config.MapHttpAttributeRoutes();

            // Remove the XML formatter
            // Will this cause problems with the existing webservice?
            // Only used by HttpPost-only UploadController and UploadBrokerController
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Configure the JSON formatter to suppress null values
            // and to always emit dates in UTC format
            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                Converters = new List<JsonConverter> { new DecimalJsonConverter() }
            };

        }

        private void InitialiseAutoMapper()
        {
            if (isAutoMapperInitialised || Mapping.Mapper != null)
            {
                return;
            }

            isAutoMapperInitialised = true;
        }
    }
}
