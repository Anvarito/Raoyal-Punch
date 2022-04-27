using UnityEngine;

public class PunchParticlesSpawner : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private SpriteRenderer PunchFlash;
    [SerializeField] private Transform GloveRight;
    [SerializeField] private Transform GloveLeft;

    private bool _chirality = false;

    public void PunchRight()
    {
        _chirality = true;
        InstantiateFlash();
    }

    public void PunchLeft()
    {
        _chirality = false;
        InstantiateFlash();
    }

    private void InstantiateFlash()
    {
        var glove = _chirality ? GloveRight : GloveLeft;
        var flash = Instantiate(PunchFlash, glove.position, Camera.main.transform.rotation);
        Destroy(flash.gameObject, 0.1f);
    }
}
