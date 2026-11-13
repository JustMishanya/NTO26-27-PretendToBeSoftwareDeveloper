using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button), typeof(AudioSource))]
public class Keyboard : MonoBehaviour
{
    private Workplace _workplace;
    private Button _button;
    private AudioSource _audioSource;
    [SerializeField] AudioClip clickSound;
    public void Init(Workplace workplace)
    {
        _workplace = workplace;
        _button = GetComponent<Button>();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = clickSound;
        _button.onClick.AddListener(onClick);
    }
    void onClick()
    {
        _workplace.AddPoints(1);
        _audioSource.Play();
    }

}