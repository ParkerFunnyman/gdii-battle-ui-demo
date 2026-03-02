using System;
using System.Collections.Generic;
using UnityEngine;

namespace CharlesEngine
{

    public class DialogChoiceLayout : MonoBehaviour, IDialogChoiceLayout
    {
        public GameObject AnswerLinePrefab;
        private List<IAnswerLine> _answerLines = new List<IAnswerLine>();
        public List<IAnswerLine> Lines => _answerLines;

        public float StartY = -264f;
        public float LineHeight = 68f;
        [Tooltip("This will cause the answers to be shown next to each other, dividing the screen horizontally. (LineHeight will be ignored)")]
        public bool HorizontalLayout;

        public float HorizontalPadding;
        private int _count;
        public void ShowLines(int count)
        {
            ShowLinesInternal(count);
        }

        public void ShowLinesInternal(int count, bool softRefresh = false)
        {
            _count = count;
            if (count > _answerLines.Count)
            {
                AddLines(count);
            }

            if (!softRefresh)
            {
                for (int i = 0; i < _answerLines.Count; i++)
                {
                    _answerLines[i].SetActive(false);
                }
            }

            float startY = StartY;
#if UNITY_ANDROID || UNITY_IOS
			LineHeight *= 1.3f;  // EDIT THIS FOR MOBILE
#endif
            if (!HorizontalLayout)
            {
                for (int i = 0; i < count; i++)
                {
                    var line = _answerLines[i];
                    line.SetActive(true);
                    ((MonoBehaviour) line).transform.localPosition = new Vector3(0, LineHeight * (count - 1 - i) + startY, -2);
                }
            }
            else
            {
                var screenW = (float) Globals.Settings.Resolution.x;
                var widthOne = screenW / (count + 1) + HorizontalPadding;
                var widthTotal = widthOne * (count - 1);
                for (int i = 0; i < count; i++)
                {
                    var line = _answerLines[i];
                    line.SetActive(true);
                    ((MonoBehaviour) line).transform.localPosition = new Vector3(i*widthOne-widthTotal/2f, startY, -2);
                }
            }
        }
        // THIS IS JUST TO ALWAYS SHOW THE NEW LAYOUT AS YOU CHANGE PARAMS
#if UNITY_EDITOR
        private void Update()
        {
            if( _count > 0 )
             ShowLinesInternal(_count, softRefresh:true);
        }
#endif

        private void AddLines(int max)
        {
            while (_answerLines.Count < max)
            {
                var newLine = Instantiate(AnswerLinePrefab, transform);
                newLine.name = "line" + _answerLines.Count;
                _answerLines.Add(newLine.GetComponent<IAnswerLine>());
            }
        }

        public void HideAll()
        {
            for (int i = 0; i < _answerLines.Count; i++)
            {
                var child = _answerLines[i];
                child.SetActive(false);
            }
            _count = 0;
        }
    }
}