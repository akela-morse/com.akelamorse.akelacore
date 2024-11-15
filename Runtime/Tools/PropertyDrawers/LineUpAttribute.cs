using UnityEngine;

namespace Akela.Tools
{
    public sealed class LineUpAttribute : PropertyAttribute
    {
        private readonly string[] _labels;

        public LineUpAttribute(params string[] labels)
        {
            _labels = labels;
        }

        public string[] Labels => _labels;
    }
}
