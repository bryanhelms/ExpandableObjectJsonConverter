using Newtonsoft.Json;
using Xunit;

namespace ExpandableObjectJsonConverter.Tests
{
    public class ExpandableObjectJsonConverterTests
    {
        [Fact]
        public void Object_id_only_deserializes_with_id_and_not_expanded()
        {
            var json = JsonConvert.SerializeObject(new {
                firstvalue = "firsttestvalue",
                example = "exampleobjectid"
            });

            var actual = JsonConvert.DeserializeObject<TopLevelObject>(json);

            Assert.Equal("firsttestvalue", actual.FirstValue);
            Assert.NotNull(actual.Example);
            Assert.Equal("exampleobjectid", actual.Example.Id);
            Assert.False(actual.Example.IsExpanded);
        }

        [Fact]
        public void Expanded_object_deserializes_with_full_object()
        {
            var json = JsonConvert.SerializeObject(new {
                firstValue = "firsttestvalue",
                example = new {
                    id = "otherrandomid",
                    someValue = "somerandomvalue",
                    otherValue = 42
                }
            });

            var actual = JsonConvert.DeserializeObject<TopLevelObject>(json);

            Assert.Equal("firsttestvalue", actual.FirstValue);
            Assert.NotNull(actual.Example);
            Assert.True(actual.Example.IsExpanded);
            Assert.Equal("otherrandomid", actual.Example.Id);
            Assert.Equal("somerandomvalue", actual.Example.SomeValue);
            Assert.Equal(42, actual.Example.OtherValue);
        }
    }
}