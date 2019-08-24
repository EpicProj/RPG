using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;
using System.Text.RegularExpressions;

namespace Sim_FrameWork
{
    public class TextAndGraphics : Text
    {
        [SerializeField]
        private List<SpriteItem> _items = new List<SpriteItem>();
        [SerializeField]
        private List<SpriteItemPlaceholder> _sproteItemStartIndex = new List<SpriteItemPlaceholder>();
        [SerializeField]
        private string _filterText;
        [SerializeField]
        private string _lastText;
        [SerializeField]
        private bool _lastRichText;

        public override void SetVerticesDirty()
        {
            base.SetVerticesDirty();

            if (_lastText != m_Text || _lastRichText != supportRichText)
            {
                _lastText = m_Text;
                _lastRichText = supportRichText;
                _filterText = FilterText(m_Text);
            }
        }

        private string FilterText(string text)
        {
            _sproteItemStartIndex.Clear();

            if (!supportRichText)
                return text;

            StringBuilder filterText = new StringBuilder();
            var match = TGConfig.EmojiRegex.Match(text);
            string placeholder = "";
            int index = 0;
            int startIndex = 0;

            while (match.Success)
            {
                startIndex = match.Index + filterText.Length;
                var item = SpawnItem(index);
                Vector2 size = InitItem(match, item);

                placeholder = string.Format("<quad width={0} height={1}/>", size.x, size.y);

                filterText.Append(text.Substring(0, match.Index));
                filterText.Append(placeholder);
                text = text.Substring(match.Index + match.Length);
                match = TGConfig.EmojiRegex.Match(text);
                _sproteItemStartIndex.Add(new SpriteItemPlaceholder(startIndex, placeholder.Length));
                index++;
            }

            filterText.Append(text);
            return filterText.ToString();
        }

        private SpriteItem SpawnItem(int index)
        {
            if (index < _items.Count)
            {
                return _items[index];
            }
            else
            {
                GameObject itemGo = new GameObject("SpriteItem");
                itemGo.AddComponent<RectTransform>();
                itemGo.AddComponent<Image>();
                var item = itemGo.AddComponent<SpriteItem>();
                item.Init(transform);
                _items.Add(item);
                return item;
            }
        }

        private Vector2 InitItem(Match match, SpriteItem item)
        {
            string keyContent = match.Groups[0].Value;
            EmojiTypeKey type = EmojiTypeKey.S;
            string id = "";
            TGConfig.Instance.GetEmojiInfo(keyContent, ref type, ref id);
            Vector2 defaultSize = Vector2.zero;
            bool adpativeSize = false;

            switch (type)
            {
                case EmojiTypeKey.V:
                    VideoData videoData = TGConfig.Instance.GetVideoData(id);
                    item.PlayVideo(videoData.VideoClip);
                    defaultSize = videoData.DefaultSize;
                    adpativeSize = videoData.AdaptiveSize;
                    break;
                case EmojiTypeKey.S:
                    SpriteData spriteData = TGConfig.Instance.GetSpriteData(id);
                    item.SetSprite(spriteData.Sprite);
                    defaultSize = spriteData.DefaultSize;
                    adpativeSize = spriteData.AdaptiveSize;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (adpativeSize)
            {
                defaultSize = Vector2.one * fontSize;
            }

            item.SetSize(type, defaultSize);

            return defaultSize;
        }

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            m_DisableFontTextureRebuiltCallback = true;

            var setting = GetGenerationSettings(rectTransform.rect.size);

            cachedTextGenerator.PopulateWithErrors(_filterText, setting, gameObject);
            float unitsPerPixel = 1 / pixelsPerUnit;
            SetSpriteItemImage(unitsPerPixel);
            AddUIVertexQuad(toFill, unitsPerPixel);
            m_DisableFontTextureRebuiltCallback = false;
        }

        private void SetSpriteItemImage(float unitsPerPixel)
        {
            var verts = cachedTextGenerator.verts;
            int index = 0;
            for (int i = 0; i < _items.Count; i++)
            {
                if (i < _sproteItemStartIndex.Count)
                {
                    index = _sproteItemStartIndex[i].StartIndex;
                    Vector2 one = verts[index * 4].position;
                    Vector2 two = verts[index * 4 + 2].position;
                    Vector2 center = (one + two) / 2;

                    _items[i].SetPos(center * unitsPerPixel);
                    SetEmojiShowState(i, true);
                }
                else
                {
                    SetEmojiShowState(i, false);
                }
            }
        }

        private void SetEmojiShowState(int index, bool show)
        {
            _items[index].SetActive(show);
        }

        private void AddUIVertexQuad(VertexHelper toFill, float unitsPerPixel)
        {
            toFill.Clear();
            var verts = cachedTextGenerator.verts;
            UIVertex[] verTemp = new UIVertex[4];

            Vector2 offset = new Vector2(verts[0].position.x, verts[0].position.y) * unitsPerPixel;
            offset = PixelAdjustPoint(offset) - offset;
            bool needOffset = offset != Vector2.zero;
            int index = 0;

            for (int i = 0; i < _filterText.Length; i++)
            {
                if (index < _sproteItemStartIndex.Count && _sproteItemStartIndex[index].StartIndex == i)
                {
                    i += _sproteItemStartIndex[index].Length - 1;
                    index++;
                    continue;
                }

                for (int j = 0; j < 4; j++)
                {
                    verTemp[j] = verts[i * 4 + j];
                    verTemp[j].position *= unitsPerPixel;
                    if (needOffset)
                    {
                        verTemp[j].position.x += offset.x;
                        verTemp[j].position.y += offset.y;
                    }
                }

                toFill.AddUIVertexQuad(verTemp);
            }
        }

    }

    [Serializable]
    public class SpriteItemPlaceholder
    {
        public int StartIndex;
        public int Length;

        public SpriteItemPlaceholder(int startIndex, int length)
        {
            StartIndex = startIndex;
            Length = length;
        }
    }

}