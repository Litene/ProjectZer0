
namespace Interactable.PickUp
{
    public class PickUp : Interactable
    {
        public Item ItemPickup;

        private void Update()
        {
            if (ItemPickup.isDisabled) {
                this.gameObject.SetActive(false);
            }
        }
    }
}

