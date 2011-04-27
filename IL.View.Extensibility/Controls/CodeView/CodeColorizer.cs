﻿using System.IO;
using System.Text;

namespace IL.View.Controls.CodeView
{
  /// <summary>
  /// Colorizes source code.
  /// </summary>
  internal class CodeColorizer : ICodeColorizer
  {
    private readonly ILanguageParser languageParser;

    /// <summary>
    /// Initializes a new instance of the <see cref="CodeColorizer"/> class.
    /// </summary>
    public CodeColorizer()
    {
      languageParser = new LanguageParser(new LanguageCompiler(Languages.CompiledLanguages), Languages.LanguageRepository);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CodeColorizer"/> class.
    /// </summary>
    /// <param name="languageParser">The language parser that the <see cref="CodeColorizer"/> instance will use for its lifetime.</param>
    public CodeColorizer(ILanguageParser languageParser)
    {
      Guard.ArgNotNull(languageParser, "languageParser");
      this.languageParser = languageParser;
    }

    /// <summary>
    /// Colorizes source code using the specified language, the default formatter, and the default style sheet.
    /// </summary>
    /// <param name="sourceCode">The source code to colorize.</param>
    /// <param name="language">The language to use to colorize the source code.</param>
    /// <returns>The colorized source code.</returns>
    public string Colorize(string sourceCode, ILanguage language)
    {
      var buffer = new StringBuilder(sourceCode.Length * 2);

      using (TextWriter writer = new StringWriter(buffer))
      {
        Colorize(sourceCode, language);
      }

      return buffer.ToString();
    }

    /// <summary>
    /// Colorizes source code using the specified language, formatter, and style sheet.
    /// </summary>
    /// <param name="sourceCode">The source code to colorize.</param>
    /// <param name="language">The language to use to colorize the source code.</param>
    /// <param name="formatter">The formatter to use to colorize the source code.</param>
    /// <param name="styleSheet">The style sheet to use to colorize the source code.</param>
    public void Colorize(string sourceCode, ILanguage language, IFormatter formatter, IStyleSheet styleSheet)
    {
      Guard.ArgNotNull(language, "language");
      Guard.ArgNotNull(formatter, "formatter");
      Guard.ArgNotNull(styleSheet, "styleSheet");

      languageParser.Parse(sourceCode, language, (parsedSourceCode, captures) => formatter.Write(parsedSourceCode, captures, styleSheet));
    }
  }
}
