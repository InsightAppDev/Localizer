using System;

namespace Insight.Localizer.Providers
{
    public readonly struct BlockCultureData : IEquatable<BlockCultureData>
    {
        public BlockCultureData(string block, string culture, string content)
        {
            Block = block;
            Culture = culture;
            Content = content;
        }

        public string Block { get; }

        public string Culture { get; }

        public string Content { get; }

        public bool Equals(BlockCultureData other)
        {
            return Block == other.Block && Culture == other.Culture && Content == other.Content;
        }

        public override bool Equals(object obj)
        {
            return obj is BlockCultureData other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Block, Culture, Content);
        }
    }
}