// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.Docs.Build
{
    internal class TableOfContentsInputItem
    {
        [JsonRequired]
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Href { get; set; }

        public string TopicHref { get; set; }

        public string TocHref { get; set; }

        [JsonExtensionData]
        public JObject Metadata { get; set; }

        public bool MaintainContext { get; set; }

        public bool Expanded { get; set; }

        [MinLength(1)]
        public List<TableOfContentsInputItem> Items { get; set; }

        public static TableOfContentsItem ToTableOfContentsModel(TableOfContentsInputItem inputModel)
        {
            if (inputModel == null)
            {
                return null;
            }

            var decodedHref = inputModel.Href == null ? null : HttpUtility.UrlDecode(inputModel.Href);
            return new TableOfContentsItem
            {
                TocTitle = inputModel.DisplayName ?? inputModel.Name,

                // TODO: legacy only, we should not touch href
                Href = decodedHref?.ToLowerInvariant(),
                Metadata = inputModel.Metadata,
                Children = inputModel.Items?.Select(l => ToTableOfContentsModel(l)).ToList(),
            };
        }
    }
}