﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServerlessFunc
{
    /// <summary>
    /// A class for interacting with the download API.
    /// </summary>
    public class DownloadApi
    {
        private readonly HttpClient _entityClient;
        private readonly string _sessionRoute;
        private readonly string _submissionRoute;
        private readonly string _analysisRoute;

        /// <summary>
        /// Constructs a new DownloadApi instance.
        /// </summary>
        /// <param name="sessionRoute">The base URL for the session route.</param>
        /// <param name="submissionRoute">The base URL for the submission route.</param>
        /// <param name="analysisRoute">The base URL for the analysis route.</param>
        public DownloadApi( string sessionRoute , string submissionRoute , string analysisRoute )
        {
            _entityClient = new HttpClient();
            _sessionRoute = sessionRoute;
            _submissionRoute = submissionRoute;
            _analysisRoute = analysisRoute;
        }

        /// <summary>
        /// Retrieves a list of session entities for the specified host username.
        /// </summary>
        /// <param name="hostUsername">The host username to filter sessions by.</param>
        /// <returns>A collection of session entities.</returns>
        public async Task<IReadOnlyList<SessionEntity>> GetSessionsByHostNameAsync( string hostUsername )
        {
            HttpResponseMessage response = await _entityClient.GetAsync( _sessionRoute + $"/{hostUsername}" );
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true ,

            };

            IReadOnlyList<SessionEntity> entities = System.Text.Json.JsonSerializer.Deserialize<IReadOnlyList<SessionEntity>>( result , options );
            return entities;
        }

        /// <summary>
        /// Retrieves the submission content associated with the specified username and session ID.
        /// </summary>
        /// <param name="username">The username of the submission.</param>
        /// <param name="sessionId">The session ID of the submission.</param>
        /// <returns>The byte array representing the submission content.</returns>
        public async Task<byte[]> GetSubmissionByUserNameAndSessionIdAsync( string username , string sessionId )
        {
            HttpResponseMessage response = await _entityClient.GetAsync( _submissionRoute + $"/{sessionId}/{username}" );
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true ,

            };

            byte[] submission = JsonSerializer.Deserialize<byte[]>( result , options );
            return submission;
        }

        /// <summary>
        /// Retrieves the analysis results associated with the specified username and session ID.
        /// </summary>
        /// <param name="username">The username of the analysis.</param>
        /// <param name="sessionId">The session ID of the analysis.</param>
        /// <returns>A collection of AnalysisEntity objects representing the analysis results.</returns>
        public async Task<IReadOnlyList<AnalysisEntity>> GetAnalysisByUserNameAndSessionIdAsync( string username , string sessionId )
        {
            HttpResponseMessage response = await _entityClient.GetAsync( _analysisRoute + $"/{sessionId}/{username}" );
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true ,

            };

            IReadOnlyList<AnalysisEntity> entities = System.Text.Json.JsonSerializer.Deserialize<IReadOnlyList<AnalysisEntity>>( result , options );
            return entities;
        }

        /// <summary>
        /// Retrieves the analysis results associated with the specified session ID.
        /// </summary>
        /// <param name="sessionId">The session ID of the analysis.</param>
        /// <returns>A collection of AnalysisEntity objects representing the analysis results.</returns>
        public async Task<IReadOnlyList<AnalysisEntity>> GetAnalysisBySessionIdAsync( string sessionId )
        {
            HttpResponseMessage response = await _entityClient.GetAsync( _analysisRoute + $"/{sessionId}" );
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true ,

            };

            IReadOnlyList<AnalysisEntity> entities = System.Text.Json.JsonSerializer.Deserialize<IReadOnlyList<AnalysisEntity>>( result , options );
            return entities;
        }

        /// <summary>
        /// Deletes all sessions from the database.
        /// </summary>
        public async Task DeleteAllSessionsAsync()
        {
            try
            {
                using HttpResponseMessage response = await _entityClient.DeleteAsync( _sessionRoute );
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Trace.WriteLine( "[cloud] Network Error Exception " + ex );
            }
        }

        /// <summary>
        /// Deletes all submissions from the database.
        /// </summary>
        public async Task DeleteAllSubmissionsAsync()
        {
            try
            {
                using HttpResponseMessage response = await _entityClient.DeleteAsync( _submissionRoute );
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Trace.WriteLine( "[cloud] Network Error Exception " + ex );
            }
        }

        /// <summary>
        /// Deletes all analysis results from the database.
        /// </summary>
        public async Task DeleteAllAnalysisAsync()
        {
            try
            {
                using HttpResponseMessage response = await _entityClient.DeleteAsync( _analysisRoute );
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Trace.WriteLine( "[cloud] Network Error Exception " + ex );
            }
        }
    }
}