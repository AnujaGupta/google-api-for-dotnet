//-----------------------------------------------------------------------
// <copyright file="GnewsSearchClient.cs" company="iron9light">
// Copyright (c) 2009 iron9light
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// </copyright>
// <author>iron9light@gmail.com</author>
//-----------------------------------------------------------------------

namespace Google.API.Search
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    /// <summary>
    /// The client for news search.
    /// </summary>
    public class GnewsSearchClient : GSearchClient
    {
        /// <summary>
        /// Search news.
        /// </summary>
        /// <param name="keyword">The keyword.</param>
        /// <param name="resultCount">The count of result itmes.</param>
        /// <returns>The result items.</returns>
        /// <remarks>Now, the max count of items Google given is <b>32</b>.</remarks>
        public IList<INewsResult> Search(string keyword, int resultCount)
        {
            return this.Search(keyword, resultCount, null, new SortType());
        }

        /// <summary>
        /// Search news.
        /// </summary>
        /// <param name="keyword">The keyword.</param>
        /// <param name="resultCount">The count of result itmes.</param>
        /// <param name="sortBy">The way to order results.</param>
        /// <returns>The result items.</returns>
        /// <remarks>Now, the max count of items Google given is <b>32</b>.</remarks>
        public IList<INewsResult> Search(string keyword, int resultCount, SortType sortBy)
        {
            return this.Search(keyword, resultCount, null, sortBy);
        }

        /// <summary>
        /// Search news.
        /// </summary>
        /// <param name="keyword">The keyword.</param>
        /// <param name="resultCount">The count of result itmes.</param>
        /// <param name="geo">A particular location of the news. You must supply either a city, state, country, or zip code as in "Santa Barbara" or "British Columbia" or "Peru" or "93108".</param>
        /// <returns>The result items.</returns>
        /// <remarks>Now, the max count of items Google given is <b>32</b>.</remarks>
        public IList<INewsResult> Search(string keyword, int resultCount, string geo)
        {
            return this.Search(keyword, resultCount, geo, new SortType());
        }

        /// <summary>
        /// Search news.
        /// </summary>
        /// <param name="keyword">The keyword.</param>
        /// <param name="resultCount">The count of result itmes.</param>
        /// <param name="geo">A particular location of the news. You must supply either a city, state, country, or zip code as in "Santa Barbara" or "British Columbia" or "Peru" or "93108".</param>
        /// <param name="sortBy">The way to order results.</param>
        /// <returns>The result items.</returns>
        /// <remarks>Now, the max count of items Google given is <b>32</b>.</remarks>
        public IList<INewsResult> Search(string keyword, int resultCount, string geo, SortType sortBy)
        {
            return this.Search(keyword, resultCount, geo, sortBy, null, null, null);
        }

        /// <summary>
        /// Search news.
        /// </summary>
        /// <param name="keyword">The keyword.</param>
        /// <param name="resultCount">The count of result itmes.</param>
        /// <param name="geo">A particular location of the news. You must supply either a city, state, country, or zip code as in "Santa Barbara" or "British Columbia" or "Peru" or "93108".</param>
        /// <param name="sortBy">The way to order results.</param>
        /// <param name="quoteId">This optional argument tells the news search system to scope search results to include only quote typed results.</param>
        /// <param name="topic">This optional argument tells the news search system to scope search results to a particular topic.</param>
        /// <param name="edition">This optional argument tells the news search system which edition of news to pull results from.</param>
        /// <returns>The result items.</returns>
        /// <remarks>Now, the max count of items Google given is <b>32</b>.</remarks>
        public IList<INewsResult> Search(
            string keyword,
            int resultCount,
            [Optional] string geo,
            [Optional] SortType sortBy,
            [Optional] string quoteId,
            [Optional] string topic,
            [Optional] string edition)
        {
            if (keyword == null && string.IsNullOrEmpty(geo))
            {
                throw new ArgumentNullException("keyword");
            }

            GSearchCallback<GnewsResult> gsearch =
                (start, resultSize) => this.GSearch(keyword, start, resultSize, geo, sortBy, quoteId, topic, edition);
            var results = SearchUtility.Search(gsearch, resultCount);
            return results.ConvertAll(item => (INewsResult)item);
        }

        /// <summary>
        /// Search the latest local news.
        /// </summary>
        /// <param name="geo">A particular location of the news. You must supply either a city, state, country, or zip code as in "Santa Barbara" or "British Columbia" or "Peru" or "93108".</param>
        /// <param name="resultCount">The count of result itmes.</param>
        /// <returns>The result items.</returns>
        /// <remarks>Now, the max count of items Google given is <b>32</b>.</remarks>
        public IList<INewsResult> SearchLocal(string geo, int resultCount)
        {
            return this.SearchLocal(geo, resultCount, new SortType());
        }

        /// <summary>
        /// Search the latest local news.
        /// </summary>
        /// <param name="geo">A particular location of the news. You must supply either a city, state, country, or zip code as in "Santa Barbara" or "British Columbia" or "Peru" or "93108".</param>
        /// <param name="resultCount">The count of result itmes.</param>
        /// <param name="sortBy">The way to order results.</param>
        /// <returns>The result items.</returns>
        /// <remarks>Now, the max count of items Google given is <b>32</b>.</remarks>
        public IList<INewsResult> SearchLocal(string geo, int resultCount, SortType sortBy)
        {
            if (geo == null)
            {
                throw new ArgumentNullException("geo");
            }

            return this.Search(null, resultCount, geo, sortBy);
        }

        internal SearchData<GnewsResult> GSearch(
            string keyword,
            int start,
            ResultSize resultSize,
            string geo,
            SortType sortBy,
            string quoteId,
            string topic,
            string edition)
        {
            if (keyword == null && string.IsNullOrEmpty(geo) && string.IsNullOrEmpty(topic))
            {
                throw new ArgumentNullException("keyword");
            }

            var responseData =
                this.GetResponseData(
                    service =>
                    service.NewsSearch(
                        this.AcceptLanguage,
                        this.ApiKey,
                        keyword,
                        resultSize.ToString(),
                        start,
                        sortBy.GetString(),
                        geo,
                        quoteId,
                        topic,
                        edition));
            return responseData;
        }
    }
}