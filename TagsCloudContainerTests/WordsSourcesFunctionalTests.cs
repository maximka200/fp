using System.Text;
using FluentAssertions;
using TagsCloudContainer.Core;
using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Core.WordSources;

namespace TagsCloudContainerTests;

[SetUpFixture]
public class EncodingSetup
{
    [OneTimeSetUp]
    public void RegisterEncodings()
        => Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
}

[TestFixture]
public class WordsSourcesFunctionalTests
{
    [Test]
    public void DocxWordsSource_GetWords_ShouldTokenizeTextFromDocxFixture()
    {
        var path = TestDataPath("words.docx");
        File.Exists(path).Should().BeTrue();

        IWordsSource source = new DocxWordsSource();

        var result = source.GetWords(path);
        
        result.IsSuccess.Should().BeTrue();
        
        var words = result.Value.ToArray();

        words.Should().Contain(["Hello", "world", "hello", "cloud", "2025"]);
    }

    [Test]
    public void DocWordsSource_GetWords_ShouldTokenizeTextFromDocFixture()
    {
        var path = TestDataPath("words.doc");
        File.Exists(path).Should().BeTrue();

        var source = new DocWordsSource();
        
        var result = source.GetWords(path);
        
        result.IsSuccess.Should().BeTrue();
        
        var words = result.Value.ToArray();
        words.Should().Contain(new[] { "Hello", "world", "hello", "cloud", "2025" });
    }


    [Test]
    public void WordsSourceFactory_Create_ShouldPickSourceThatCanHandle_FormatIsTrimmed_AndCaseIsIgnored()
    {
        var sources = DefaultSources();
        
        var settings = new SourceSettings("whatever", "  DOCX  ");

        var result = WordsSourceFactory.Create(settings, sources);
        result.IsSuccess.Should().BeTrue();

        var source = result.Value;
        
        source.Should().BeOfType<DocxWordsSource>();
    }

    [TestCase("txt", typeof(TxtWordsSource))]
    [TestCase("doc", typeof(DocWordsSource))]
    [TestCase("docx", typeof(DocxWordsSource))]
    [TestCase("DOCX", typeof(DocxWordsSource))]
    public void WordsSourceFactory_Create_ShouldReturnExpectedImplementation(string format, Type expectedType)
    {
        var sources = DefaultSources();
        var settings = new SourceSettings("whatever", format);

        var result = WordsSourceFactory.Create(settings, sources);
        
        result.IsSuccess.Should().BeTrue();
        
        var source = result.Value;
        
        source.Should().BeOfType(expectedType);
    }

    [Test]
    public void WordsSourceFactory_Create_WhenFormatIsUnsupported_ShouldFailure()
    {
        var sources = DefaultSources();
        const string format = "pdf";
        
        var result = WordsSourceFactory.Create(new SourceSettings("whatever", format), sources);
        
        result.IsSuccess.Should().Be(false);
        result.Error.Should().Be($"No suitable words source found for the given format: {format}");
    }

    [Test]
    public void WordsSourceFactory_Create_WhenSourcesAreEmpty_ShouldFailure()
    {
        const string format = "txt";
        
        var result = WordsSourceFactory.Create(new SourceSettings("whatever", format), Array.Empty<IWordsSource>());
        
        result.IsSuccess.Should().Be(false);
        result.Error.Should().Be($"No suitable words source found for the given format: {format}");
    }

    private static IWordsSource[] DefaultSources() =>
    [
        new TxtWordsSource(),
        new DocWordsSource(),
        new DocxWordsSource()
    ];

    private static string TestDataPath(string fileName) =>
        Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", fileName);
}
