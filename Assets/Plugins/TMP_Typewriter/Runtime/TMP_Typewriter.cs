using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace KoganeUnityLib
{
    /// <summary>
    /// TextMesh Pro で 1 文字ずつ表示する演出を再生するコンポーネント
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public partial class TMP_Typewriter : MonoBehaviour
    {
        //==============================================================================
        // 変数(SerializeField)
        //==============================================================================
        // [SerializeField]
        public TextMeshProUGUI m_TMP_Text { get; private set; }

        //==============================================================================
        // 変数
        //==============================================================================
        private string m_parsedText;
        private Action m_onComplete;
        private Tween m_tween;

        // private List<(int index, int count)> m_rubyInfos = new(32);
        private List<(int index, int count)> m_rubyInfos = new List<(int index, int count)>(32);

        //==============================================================================
        // 関数
        //==============================================================================
        /// <summary>
        /// アタッチされた時や Reset された時に呼び出されます
        /// </summary>
        private void Reset()
        {
            m_TMP_Text = GetComponent<TextMeshProUGUI>();
        }

        private void Awake()
        {
            m_TMP_Text = GetComponent<TextMeshProUGUI>();
        }

        /// <summary>
        /// 破棄される時に呼び出されます
        /// </summary>
        private void OnDestroy()
        {
            if (m_tween != null)
                m_tween.Kill();

            m_tween = null;
            m_onComplete = null;
        }

        /// <summary>
        /// 演出を再生します（ルビ対応版）
        /// </summary>
        /// <param name="text">表示するテキスト ( リッチテキスト対応 )</param>
        /// <param name="speed">表示する速さ ( speed == 1 の場合 1 文字の表示に 1 秒、speed == 2 の場合 0.5 秒かかる )</param>
        /// <param name="onComplete">演出完了時に呼び出されるコールバック</param>
        /// <param name="fixedLineHeight">ルビ表示用に行間を固定する</param>
        /// <param name="autoMarginTop">1行目にルビがある時はMarginTopで位置調整する</param>
        public void Play(
            string text,
            float speed,
            Action onComplete,
            bool fixedLineHeight = false,
            bool autoMarginTop = true
        )
        {
            m_TMP_Text.text = text;
            m_TMP_Text.ForceMeshUpdate();
            m_onComplete = onComplete;

            // ルビタグ展開前のリッチテキスト除外テキストを取得
            m_parsedText = m_TMP_Text.GetParsedText();
            SetRubyInfos(m_parsedText);

            m_parsedText = TMProRubyUtil.RemoveRubyTag(m_parsedText);
            int length = m_parsedText.Length;
            float duration = 1 / speed * length;

            m_TMP_Text.SetTextAndExpandRuby(text, fixedLineHeight, autoMarginTop);
            m_TMP_Text.ForceMeshUpdate();

            OnUpdate(0);

            if (m_tween != null)
                m_tween.Kill();

            m_tween = DOTween
                .To(value => OnUpdate(value), 0, 1, duration)
                .SetEase(Ease.Linear)
                .OnComplete(() => OnComplete());
        }

        /// <summary>
        /// ルビタグごとの漢字の終了位置（ルビタグを除外した位置）と、ルビの文字数を取得
        /// </summary>
        public void SetRubyInfos(string text)
        {
            m_rubyInfos.Clear();
            Match match = TMProRubyUtil.TagRegex.Match(text);

            while (match.Success)
            {
                if (match.Groups.Count > 2)
                {
                    Group ruby = match.Groups["ruby"];
                    Group kanji = match.Groups["kanji"];
                    m_rubyInfos.Add((ruby.Index + kanji.Value.Length - 3, ruby.Value.Length));
                    text = text.Replace(match.Groups[0].Value, kanji.Value);
                    match = TMProRubyUtil.TagRegex.Match(text);
                }
                else
                {
                    match.NextMatch();
                }
            }
        }

        /// <summary>
        /// 演出をスキップします
        /// </summary>
        /// <param name="withCallbacks">演出完了時に呼び出されるコールバックを実行する場合 true</param>
        public void Skip(bool withCallbacks = true)
        {
            if (m_tween != null)
                m_tween.Kill();

            m_tween = null;

            OnUpdate(1);

            if (!withCallbacks)
                return;

            m_onComplete?.Invoke();

            m_onComplete = null;
        }

        /// <summary>
        /// 演出を一時停止します
        /// </summary>
        public void Pause()
        {
            if (m_tween != null)
                m_tween.Pause();
        }

        /// <summary>
        /// 演出を再開します
        /// </summary>
        public void Resume()
        {
            if (m_tween != null)
                m_tween.Play();
        }

        /// <summary>
        /// 演出を更新する時に呼び出されます
        /// </summary>
        private void OnUpdate(float value)
        {
            float current = Mathf.Lerp(0, m_parsedText.Length, value);
            int count = Mathf.FloorToInt(current);
            int rubyAddedCount = count;

            foreach ((int index, int count) info in m_rubyInfos)
            {
                if (count >= info.index)
                    rubyAddedCount += info.count;
            }

            m_TMP_Text.maxVisibleCharacters = rubyAddedCount;

            // Debug.Log(value);
        }

        /// <summary>
        /// 演出が更新した時に呼び出されます
        /// </summary>
        private void OnComplete()
        {
            m_tween = null;

            m_onComplete?.Invoke();

            m_onComplete = null;
        }
    }
}
