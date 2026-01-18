using System.Text;
using FluentAssertions;
using TagsCloudContainer.Core;
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

        var words = source.GetWords(path).Value.ToArray();

        words.Should().Contain(["Hello", "world", "hello", "cloud", "2025"]);
    }

    [Test]
    public void DocWordsSource_GetWords_ShouldTokenizeTextFromDocFixture()
    {
        var path = TestDataPath("words.doc");
        File.Exists(path).Should().BeTrue("файл words.doc должен быть в TestData");

        var source = new DocWordsSource();

        var words = source.GetWords(path).Value.ToArray();

        words.Should().Contain(["Hello", "world", "hello", "cloud", "2025"]);
    }

    [Test]
    public void WordsSourceFactory_Create_ShouldPickSourceThatCanHandle_FormatIsTrimmed_AndCaseIsIgnored()
    {
        var sources = DefaultSources();
        
        var settings = new SourceSettings("whatever", "  DOCX  ");

        var source = WordsSourceFactory.Create(settings, sources).Value;

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

        var source = WordsSourceFactory.Create(settings, sources).Value;

        source.Should().BeOfType(expectedType);
    }

    [Test]
    public void WordsSourceFactory_Create_WhenFormatIsUnsupported_ShouldFailure()
    {
        var sources = DefaultSources();
        var result = WordsSourceFactory
            .Create(new SourceSettings("whatever", "pdf"), sources);

        result.IsSuccess.Should().Be(false);
    }

    [Test]
    public void WordsSourceFactory_Create_WhenSourcesAreEmpty_ShouldFailure()
    {
        var result = WordsSourceFactory.Create(new SourceSettings("whatever", "txt"), Array.Empty<IWordsSource>());

        result.IsSuccess.Should().Be(false);
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
