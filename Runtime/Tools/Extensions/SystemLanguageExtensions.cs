using System;
using System.Globalization;
using UnityEngine;

namespace Akela.Tools
{
	public static class SystemLanguageExtensions
	{
		public static SystemLanguage ToSystemLanguage(this string input)
		{
			return (SystemLanguage)Enum.Parse(typeof(SystemLanguage), input);
		}

		public static CultureInfo ToCultureInfo(this SystemLanguage sysLanguage)
		{
			return sysLanguage switch
			{
				SystemLanguage.Afrikaans => new CultureInfo("af-ZA"),
				SystemLanguage.Arabic => new CultureInfo("ar-SA"),
				SystemLanguage.Basque => new CultureInfo("eu-ES"),
				SystemLanguage.Belarusian => new CultureInfo("be-BY"),
				SystemLanguage.Bulgarian => new CultureInfo("bg-BG"),
				SystemLanguage.Catalan => new CultureInfo("ca-ES"),
				SystemLanguage.Chinese => new CultureInfo("zh-CN"),
				SystemLanguage.Czech => new CultureInfo("cs-CZ"),
				SystemLanguage.Danish => new CultureInfo("da-DK"),
				SystemLanguage.Dutch => new CultureInfo("nl-NL"),
				SystemLanguage.English => new CultureInfo("en-US"),
				SystemLanguage.Estonian => new CultureInfo("et-EE"),
				SystemLanguage.Faroese => new CultureInfo("fo-FO"),
				SystemLanguage.Finnish => new CultureInfo("fi-FI"),
				SystemLanguage.French => new CultureInfo("fr-FR"),
				SystemLanguage.German => new CultureInfo("de-DE"),
				SystemLanguage.Greek => new CultureInfo("el-GR"),
				SystemLanguage.Hebrew => new CultureInfo("he-IL"),
				SystemLanguage.Hungarian => new CultureInfo("hu-HU"),
				SystemLanguage.Icelandic => new CultureInfo("is-IS"),
				SystemLanguage.Indonesian => new CultureInfo("id-ID"),
				SystemLanguage.Italian => new CultureInfo("it-IT"),
				SystemLanguage.Japanese => new CultureInfo("ja-JP"),
				SystemLanguage.Korean => new CultureInfo("ko-KR"),
				SystemLanguage.Latvian => new CultureInfo("lv-LV"),
				SystemLanguage.Lithuanian => new CultureInfo("lt-LT"),
				SystemLanguage.Norwegian => new CultureInfo("nn-NO"),
				SystemLanguage.Polish => new CultureInfo("pl-PL"),
				SystemLanguage.Portuguese => new CultureInfo("pt-PT"),
				SystemLanguage.Romanian => new CultureInfo("ro-RO"),
				SystemLanguage.Russian => new CultureInfo("ru-RU"),
				SystemLanguage.SerboCroatian => new CultureInfo("hr-HR"),
				SystemLanguage.Slovak => new CultureInfo("sk-SK"),
				SystemLanguage.Slovenian => new CultureInfo("sl-SI"),
				SystemLanguage.Spanish => new CultureInfo("es-ES"),
				SystemLanguage.Swedish => new CultureInfo("sv-SE"),
				SystemLanguage.Thai => new CultureInfo("th-TH"),
				SystemLanguage.Turkish => new CultureInfo("tr-TR"),
				SystemLanguage.Ukrainian => new CultureInfo("uk-UA"),
				SystemLanguage.Vietnamese => new CultureInfo("vi-VN"),
				_ => CultureInfo.InvariantCulture,
			};
		}

		public static SystemLanguage ToSystemLanguage(this CultureInfo cultureInfo)
		{
			return cultureInfo.Name switch
			{
				"af-ZA" => SystemLanguage.Afrikaans,
				"ar-SA" => SystemLanguage.Arabic,
				"eu-ES" => SystemLanguage.Basque,
				"be-BY" => SystemLanguage.Belarusian,
				"bg-BG" => SystemLanguage.Bulgarian,
				"ca-ES" => SystemLanguage.Catalan,
				"zh-CN" => SystemLanguage.Chinese,
				"cs-CZ" => SystemLanguage.Czech,
				"da-DK" => SystemLanguage.Danish,
				"nl-NL" => SystemLanguage.Dutch,
				"en-US" => SystemLanguage.English,
				"et-EE" => SystemLanguage.Estonian,
				"fo-FO" => SystemLanguage.Faroese,
				"fi-FI" => SystemLanguage.Finnish,
				"fr-FR" => SystemLanguage.French,
				"de-DE" => SystemLanguage.German,
				"el-GR" => SystemLanguage.Greek,
				"he-IL" => SystemLanguage.Hebrew,
				"hu-HU" => SystemLanguage.Hungarian,
				"is-IS" => SystemLanguage.Icelandic,
				"id-ID" => SystemLanguage.Indonesian,
				"it-IT" => SystemLanguage.Italian,
				"ja-JP" => SystemLanguage.Japanese,
				"ko-KR" => SystemLanguage.Korean,
				"lv-LV" => SystemLanguage.Latvian,
				"lt-LT" => SystemLanguage.Lithuanian,
				"nn-NO" => SystemLanguage.Norwegian,
				"pl-PL" => SystemLanguage.Polish,
				"pt-PT" => SystemLanguage.Portuguese,
				"ro-RO" => SystemLanguage.Romanian,
				"ru-RU" => SystemLanguage.Russian,
				"hr-HR" => SystemLanguage.SerboCroatian,
				"sk-SK" => SystemLanguage.Slovak,
				"sl-SI" => SystemLanguage.Slovenian,
				"es-ES" => SystemLanguage.Spanish,
				"sv-SE" => SystemLanguage.Swedish,
				"th-TH" => SystemLanguage.Thai,
				"tr-TR" => SystemLanguage.Turkish,
				"uk-UA" => SystemLanguage.Ukrainian,
				"vi-VN" => SystemLanguage.Vietnamese,
				_ => SystemLanguage.Unknown,
			};
		}
	}
}