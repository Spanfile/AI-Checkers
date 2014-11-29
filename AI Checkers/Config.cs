﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AI_Checkers
{
    public static class Config
    {
        static Dictionary<string, int> integers;
        static Dictionary<string, string> strings;

        static Config()
        {
            integers = new Dictionary<string, int>();
            strings = new Dictionary<string, string>();
        }

        public static int GetInteger(string name)
        {
            if (integers.ContainsKey(name))
                return integers[name];

            throw new ArgumentException();
        }

        public static string GetString(string name)
        {
            if (strings.ContainsKey(name))
                return strings[name];

            throw new ArgumentException();
        }

        public static void LoadConfigFile(string filepath)
        {
            if (!File.Exists(filepath))
            {
                Console.WriteLine("Config: Failed to load config values, file does not exist (\"{0\")", filepath);
                return;
            }

            int index = -1;
            foreach (var line in File.ReadLines(filepath))
            {
                index += 1;

                if (String.IsNullOrWhiteSpace(line) || line.StartsWith("#")) // ignored lines
                    continue;

                var args = line.Split(new string[] { " = " }, StringSplitOptions.None);

                if (args.Length != 2)
                {
                    Console.WriteLine("Config: Invalid line {0} (wrong amount of arguments, wanted 2, got {1})", index, args.Length);
                    continue;
                }

                var typeArgs = args[0].Split(new string[] { " " }, StringSplitOptions.None);
                var argValue = args[1];

                if (typeArgs.Length != 2)
                {
                    Console.WriteLine("Config: Invalid line {0} (wrong amount of type arguments, wanted 2, got {1})", index, typeArgs.Length);
                    continue;
                }

                var type = typeArgs[0];
                var name = typeArgs[1];

                switch (type)
                {
                    default:
                        Console.WriteLine("Config: Invalid line {0} (wrong type, wanted \"string\" or \"int\", got \"{1}\")", index, type);
                        continue;

                    case "int":
                        var intValue = 0;

                        if (!Int32.TryParse(argValue, out intValue))
                        {
                            Console.WriteLine("Config: Invalid line {0} (wrong value type, wanted {1}, got \"{2}\")", index, type, argValue);
                            continue;
                        }

                        integers.Add(name, intValue);
                        break;

                    case "string":
                        strings.Add(name, argValue);
                        break;
                }
            }

            Console.WriteLine("Config: {0} integer values and {1} string values loaded from \"{2}\"", integers.Count, strings.Count, Path.GetFileName(filepath));
        }
    }
}
