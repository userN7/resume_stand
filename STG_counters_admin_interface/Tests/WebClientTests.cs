using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using STG_counters_admin_interface;
using System.Net.Http;
using Xunit;
using System.Text;
using AngleSharp.Html.Dom;
using AngleSharp;
using System.Threading;
using AngleSharp.Io;
using System.Net.Http.Headers;
using static STG_counters_admin_interface.Models.Constants;


namespace STG_counters_admin_interface.Tests
{
    public class WebClientTests
    : IClassFixture<WebApplicationFactory<STG_counters_admin_interface.Startup>>
    {
        private readonly WebApplicationFactory<STG_counters_admin_interface.Startup> _factory;
        string host = "https://localhost:5001";
        string start_Date = "2020-06-01";
        string end_Date = "2020-06-01";
        public static readonly IEnumerable<object[]> List_report_fullname = List_report_fullname_for_tests();
        public WebClientTests(WebApplicationFactory<STG_counters_admin_interface.Startup> factory)
        {
            _factory = factory;
        }

        async Task Check_web_connection_status_code(string url, string content_type_to_check = "text/html; charset=utf-8")
        {
            // Arrange
            var client = _factory.CreateClient();
            // Act
            var response = await client.GetAsync(url);
            // Assert
            try
            {
                response.EnsureSuccessStatusCode();// Status Code 200-299

                Assert.Equal(content_type_to_check,
                response.Content.Headers.ContentType.ToString());
            }
            catch (Exception e)
            {
                throw e;
            };
        }

        [Fact]
         async Task Curr_test()
        {


            var start_Date = "2019-08-01";
            var end_Date = "2019-08-31";
            string url = $@"{host}/Admin/Action_handler?start_Date={start_Date}&end_Date={end_Date}&action_number=6";
            await Check_web_connection_status_code(url);



        }

        [Theory]
        [MemberData(nameof(List_report_fullname))]
        public async Task AccessabilityTests_Reports(string report_fullname)
        {
                      
            string url = $@"{host}/Reports/ShowReport?start_Date={start_Date}&end_Date={end_Date}&report_name={report_fullname}";
            await Check_web_connection_status_code(url);



        }

        [Fact]
        public async Task AccessabilityTest_reports_index()=> await Check_web_connection_status_code($@"{host}/Reports");
        

        [Theory]
        [MemberData(nameof(List_report_fullname))]

        public async Task ExportExcel_reports(string report_fullname
            )
        {

           
            //var report_fullname = "ConsumptionByManufactureByPeriod;Общая по производству";
            string url = $@"{host}/Reports/ShowReport?start_Date={start_Date}&end_Date={end_Date}&report_name={report_fullname}&export_excel=1";
            await Check_web_connection_status_code(url, content_type_to_check: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");//проверяем возможность выгрузить эксель файл



        }


        [Theory]
        [MemberData(nameof(List_report_fullname))]
        public async Task AccessabilityTests_reports_void_data(string report_fullname)
        {
            var start_Date = DateTime.Now.AddDays(30).ToString("yyyy-MM-dd");
            var end_Date = DateTime.Now.AddDays(30).ToString("yyyy-MM-dd");
            string url = $@"{host}/Reports/ShowReport?start_Date={start_Date}&end_Date={end_Date}&report_name={report_fullname}";

            await Check_web_connection_status_code(url);
        }

        //[Fact]
        //public async Task Test_adding_fluid_stream_and_wetness()
        //{
        //    var start_Date = (new DateTime(2020, 2, 1)).ToString("yyyy-MM-dd");
        //    var end_Date = (new DateTime(2020, 2, 2)).ToString("yyyy-MM-dd");
        //    var url = $@"{host}/Admin/Action_handler?action_number=10&start_Date={start_Date}&end_Date={end_Date}";
        //    var client = _factory.CreateClient();
        //    var response = await client.GetAsync(url);
        //}

        //[Fact]
        //public async Task Test_report_full_production()
        //{
        //    var start_Date = (new DateTime(2020, 3, 1)).ToString("yyyy-MM-dd");
        //    var end_Date = (new DateTime(2020, 3, 20)).ToString("yyyy-MM-dd");
        //    var report_fullname = "ConsumptionByManufactureByPeriod;Общая по производству";
        //    var url = $@"{host}/Reports/ShowReport?start_Date={start_Date}&end_Date={end_Date}&report_name={report_fullname}"; ;
        //    var client = _factory.CreateClient();
        //    var response = await client.GetAsync(url);
        //}

        //[Fact]
        //public async Task test1()
        //{
        //    var client = _factory.CreateClient();
        //    var start_Date = DateTime.Now.AddDays(-17).ToString("yyyy-MM-dd");
        //    var end_Date = DateTime.Now.AddDays(-17).ToString("yyyy-MM-dd");
        //    string url = $@"{host}/Reports"; 
        //    //StringContent t = new StringContent($@"start_Date={start_Date}&end_Date={end_Date}&report_name=SkuDataByShifts_BDM1; По сменный отчет БДМ-1", Encoding.UTF8, "application/x-www-form-urlencoded") ;
        //    // Act
        //    //var response = await client.PostAsync(url, t);

        //    // Arrange

        //    // Act
        //    var response = await client.GetAsync(url);

        //    // Arrange

        //    var content = await HtmlHelpers.GetDocumentAsync(response);
        //    var t1 = (IHtmlFormElement)content.QuerySelector("form[id='ShowReport']");
        //    var t2 = (IHtmlInputElement)content.QuerySelector("input[id='SendButton']");
        //    var t3 = t1.Elements.Select(r => new { r.Id, r.NodeType, r.NodeValue});
        //    //Act
        //    var response1 = await client.SendAsync(
        //        t1,t2
        //        );


        //}
        [Fact]
        public async Task Copy_Year_Plan_TER()
        {

            var client = _factory.CreateClient();

            string url = $@"{host}/AdditionalData/CopyPlan";
            var response = await client.PostAsync(url,  new StringContent($"year={2020}&year_CopyTo={2021}&copy_full_Year=on", Encoding.UTF8, "application/x-www-form-urlencoded"));
            //await Check_web_connection_status_code(url);



        }
    }

    public class HtmlHelpers
    {
        public static async Task<IHtmlDocument> GetDocumentAsync(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            var document = await BrowsingContext.New()
                .OpenAsync(ResponseFactory, CancellationToken.None);
            return (IHtmlDocument)document;

            void ResponseFactory(VirtualResponse htmlResponse)
            {
                htmlResponse
                    .Address(response.RequestMessage.RequestUri)
                    .Status(response.StatusCode);

                MapHeaders(response.Headers);
                MapHeaders(response.Content.Headers);

                htmlResponse.Content(content);

                void MapHeaders(HttpHeaders headers)
                {
                    foreach (var header in headers)
                    {
                        foreach (var value in header.Value)
                        {
                            htmlResponse.Header(header.Key, value);
                        }
                    }
                }
            }
        }
    }

    public static class HttpClientExtensions//расширение HttpClient
    {
        public static Task<HttpResponseMessage> SendAsync(
            this HttpClient client,
            IHtmlFormElement form,
            IHtmlElement submitButton)
        {
            return client.SendAsync(form, submitButton, new Dictionary<string, string>());
        }

        public static Task<HttpResponseMessage> SendAsync(
            this HttpClient client,
            IHtmlFormElement form,
            IEnumerable<KeyValuePair<string, string>> formValues)
        {
            var submitElement = Assert.Single(form.QuerySelectorAll("[type=submit]"));
            var submitButton = Assert.IsAssignableFrom<IHtmlElement>(submitElement);

            return client.SendAsync(form, submitButton, formValues);
        }

        public static Task<HttpResponseMessage> SendAsync(
            this HttpClient client,
            IHtmlFormElement form,
            IHtmlElement submitButton,
            IEnumerable<KeyValuePair<string, string>> formValues)
        {
            foreach (var kvp in formValues)
            {
                var element = Assert.IsAssignableFrom<IHtmlInputElement>(form[kvp.Key]);
                element.Value = kvp.Value;
            }

            var submit = form.GetSubmission(submitButton);
            var target = (Uri)submit.Target;
            if (submitButton.HasAttribute("formaction"))
            {
                var formaction = submitButton.GetAttribute("formaction");
                target = new Uri(formaction, UriKind.Relative);
            }
            var submision = new HttpRequestMessage(new System.Net.Http.HttpMethod(submit.Method.ToString()), target)
            {
                Content = new StreamContent(submit.Body)
            };

            foreach (var header in submit.Headers)
            {
                submision.Headers.TryAddWithoutValidation(header.Key, header.Value);
                submision.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return client.SendAsync(submision);
        }
    }
}
