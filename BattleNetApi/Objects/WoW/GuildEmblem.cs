using BattleNetApi.Common.ExtensionMethods;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BattleNetApi.Objects.WoW
{
    public class GuildEmblem
    {
        #region Properties
        public int Icon { get; private set; }

        public string IconColor { get; private set; }

        public int IconColorId { get; private set; }

        public int Border { get; private set; }

        public string BorderColor { get; private set; }

        public int BorderColorId { get; private set; }

        public string BackgroundColor { get; private set; }

        public int BackgroundColorId { get; private set; }
        #endregion

        internal GuildEmblem(JObject guildEmblemJson)
        {
            AssignPrimitiveProperty(ge => ge.Icon, guildEmblemJson, "icon");
            AssignPrimitiveProperty(ge => ge.IconColorId, guildEmblemJson, "iconColorId");
            AssignPrimitiveProperty(ge => ge.IconColor, guildEmblemJson, "iconColor");
            AssignPrimitiveProperty(ge => ge.BackgroundColor, guildEmblemJson, "backgroundColor");
            AssignPrimitiveProperty(ge => ge.BackgroundColorId, guildEmblemJson, "backgroundColorId");
            AssignPrimitiveProperty(ge => ge.Border, guildEmblemJson, "border");
            AssignPrimitiveProperty(ge => ge.BorderColor, guildEmblemJson, "borderColor");
            AssignPrimitiveProperty(ge => ge.BorderColorId, guildEmblemJson, "borderColorId");
        }

        private void AssignPrimitiveProperty<TProperty>(Expression<Func<GuildEmblem, TProperty>> propertyExpression, JObject guildEmblemJson, string jsonKey)
        {
            var memberExpression = (MemberExpression)propertyExpression.Body;
            var propertyInfo = (PropertyInfo)memberExpression.Member;
            propertyInfo.SetValue(this, guildEmblemJson[jsonKey].Value<TProperty>());
        }
    }
}
