using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeBook.Models.Api
{
    public static class GenericJsonSerializerSettings
    {
        public static JsonSerializerSettings GetSettings()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.None,
                Error = (sender, args) =>
                {
                    args.ErrorContext.Handled = true;
                },
            };

            return settings;
        }
    }
}
