    using System;
    using TMPro;
    using UnityEngine;

    public class NodeMono : MonoBehaviour
    {
        private TextMeshPro m_TextMeshPro;

        private void Awake()
        {
            m_TextMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();
        }

        public void SetText(string _str)
        {
            m_TextMeshPro.text = _str;
        }
    }