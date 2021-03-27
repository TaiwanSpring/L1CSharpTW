using LineageServer.Interfaces;
using System;
using System.Globalization;
using System.Reflection;

namespace LineageServer.Language
{
    abstract class LanguageBase : ILanguage
    {
        public abstract string LanguageName { get; }

        public static ILanguage GetLanguage()
        {
            LanguageBase result = null;
            AppDomain appDomain = AppDomain.CurrentDomain;
            Assembly[] assemblies = appDomain.GetAssemblies();
            string languageName = CultureInfo.CurrentCulture.Name.ToLower();
            for (int i = 0; i < assemblies.Length; i++)
            {
                Assembly assembly = assemblies[i];
                Type[] types = assembly.GetTypes();
                for (int j = 0; j < types.Length; j++)
                {
                    if (types[j].IsAbstract && types[j].IsSubclassOf(typeof(LanguageBase)))
                    {
                        if (assembly.CreateInstance(types[j].FullName) is LanguageBase languageBase)
                        {
                            if (result == null)
                            {
                                result = languageBase;
                            }
                            if (languageBase.LanguageName.ToLower() == languageName)
                            {
                                result = languageBase;
                                break;
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual string GetString(string key)
        {
            return key;
        }
    }
}
