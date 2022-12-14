using UnityEngine;
using UnityEngine.UI;

namespace Lamplight.HUD
{
    [RequireComponent(typeof(Image))]
    public class FlashOverlayEffect : MonoBehaviour
    {
        [SerializeField] private float flashTime;

        private Image image;
        private float timer;
        private float alpha;

        private void Awake()
        {
            image = GetComponent<Image>();
            alpha = image.color.a;
        }

        public void Activate()
        {
            gameObject.SetActive(true);
            timer = flashTime;
        }

        private void Update()
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha * timer / flashTime);

            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
