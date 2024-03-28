using System.Net.Http;
using System;

namespace PirateTools.AskTheChairs.WebApp.Services;

public class BackendService {
    public HttpClient ApiClient { get; }

	public BackendService(string apiBaseUrl) {
        if (!apiBaseUrl.EndsWith('/'))
            apiBaseUrl += '/';

        ApiClient = new HttpClient {
            BaseAddress = new Uri(apiBaseUrl + "api/")
        };
    }
}