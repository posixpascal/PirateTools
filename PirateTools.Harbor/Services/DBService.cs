using PirateTools.Models.AskTheChairs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace PirateTools.Harbor.Services;

public class DBService {
    private readonly List<Question> Questions;
    private readonly Dictionary<string, int> Tokens;

    private readonly object LockObject = new();

    public DBService() {
        if (File.Exists("questions.json")) {
            Questions = JsonSerializer.Deserialize<List<Question>>(File.ReadAllText("questions.json")) ?? [];
        } else {
            Questions = [];
        }

        if (File.Exists("tokens.json")) {
            Tokens = JsonSerializer.Deserialize<Dictionary<string, int>>(File.ReadAllText("tokens.json")) ?? [];
        } else {
            Tokens = [];
        }
    }

    public string GenerateToken() {
        var token = new string(Enumerable.Range(0, 32)
            .Select(_ => (char)Random.Shared.Next(97, 123)).ToArray());

        lock (LockObject) {
            Tokens.Add(token, 5);
            Save();
        }

        return token;
    }

    public int CheckToken(string token) {
        lock (LockObject) {
            if (!Tokens.TryGetValue(token, out var usagesLeft))
                return -1;

            return usagesLeft;
        }
    }

    public void AddQuestion(string token, Question question) {
        lock (LockObject) {
            if (!Tokens.ContainsKey(token))
                return;

            question.AskedDateTime = DateTime.Now;

            Tokens[token]--;
            Questions.Add(question);
            Save();
        }
    }

    public IEnumerable<Question> GetQuestions() {
        lock (LockObject) {
            return Questions.Where(q => q.AskedDateTime.AddDays(90) > DateTime.Now)
                            .Select(q => {
                                var qc = q.Clone();
                                qc.EMail = null;
                                return qc;
                            });
        }
    }

    private void Save() {
        File.WriteAllText("questions.json", JsonSerializer.Serialize(Questions));
        File.WriteAllText("tokens.json", JsonSerializer.Serialize(Tokens));
    }
}