using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessagesDisplay : MonoBehaviour
{
    private float _animationTime = 0.5f;
    private float _showTime = 4f;
    private Queue<Message> _messageQueue = new Queue<Message>();
    private bool _isShowingMessage = false;

    // private Vector3 _paneliInitialPosition;
    [SerializeField] private GameObject _messagePanel;
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private Image _messageImage;


    public void Start()
    {
        // _paneliInitialPosition = _messagePanel.transform.position;
        _messagePanel.transform.localScale = Vector2.zero;
    }

    public void ShowMessage(string text, Sprite image)
    {
        Message newMessage = new Message(text, image);
        _messageQueue.Enqueue(newMessage);

        if (!_isShowingMessage)
        {
            StartCoroutine(ShowNextMessage());
        }
    }

    private IEnumerator ShowNextMessage()
    {
        _isShowingMessage = true;

        Message currentMessage = _messageQueue.Dequeue();

        _messageText.text = currentMessage.Text;
        _messageImage.sprite = currentMessage.Image;

        _messagePanel.transform.LeanScale(Vector2.one, _animationTime).setEaseOutBack();
        // _messagePanel.transform.localPosition = new Vector2(Screen.width, 0);
        // _messagePanel.transform.LeanMoveLocalX(0, _animationTime).setEaseOutBack();
        // _messagePanel.transform.LeanMoveLocalX(Screen.width, _animationTime).setEaseOutBack().delay = _showTime - _animationTime;
        _messagePanel.transform.LeanScale(Vector2.zero, _animationTime).setEaseOutBack().delay = _showTime - _animationTime;

        yield return new WaitForSeconds(_showTime);

        if (_messageQueue.Count > 0)
        {
            StartCoroutine(ShowNextMessage());
        }
        else
        {
            _isShowingMessage = false;
        }
    }
    private class Message
    {
        public string Text { get; }
        public Sprite Image { get; }

        public Message(string text, Sprite image)
        {
            Text = text;
            Image = image;
        }
    }
}
