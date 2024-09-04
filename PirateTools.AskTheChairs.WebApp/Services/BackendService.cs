using System.Net.Http;
using System;

namespace PirateTools.AskTheChairs.WebApp.Services;

public class BackendService {
    public HttpClient? ApiClient { get; }

	public BackendService(string apiBaseUrl) {
        if (!apiBaseUrl.EndsWith('/'))
            apiBaseUrl += '/';

        var finalUrl = apiBaseUrl + "api/";
        ApiClient = new HttpClient {
            BaseAddress = new Uri(finalUrl)
        };
    }
}