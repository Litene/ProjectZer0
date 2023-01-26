using System.Collections;
using TMPro;
using UnityEngine;

namespace Interactable.Door
{
    public class Door : Interactable
    {
        public bool IsLocked;
        public KeyCode KeyPressHintText;
        public Item KeyItem;
        private TextMeshProUGUI _worldSpaceText;
        private GameObject _thisInteractableCanvas;
        private GameObject _doorTriggerArea;
        
        private void Awake()
        {
            Transform trans = transform;
            Transform canvasTrans = trans.Find("Canvas");
            if (canvasTrans != null)
            {
                _thisInteractableCanvas = canvasTrans.gameObject;
            }

            _worldSpaceText = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Start()
        {
            GenerateTriggerAreaForDoor();
            _worldSpaceText.text = "[" + KeyPressHintText.ToString() + "]";
        }

        public void TryToOpenDoor()
        {
            if (IsLocked && Player.Instance.PlayerInventory._inventoryData.Inventory.Contains(KeyItem))
            {
                StartCoroutine(OpenDoor());
                Player.Instance.PlayerInventory.UseItem(KeyItem);
            }

            if (IsLocked && !Player.Instance.PlayerInventory._inventoryData.Inventory.Contains(KeyItem))
            {
                Debug.Log("key item for this window not present in inventory.");
                return;
            }

            if (!IsLocked)
            {
                StartCoroutine(OpenDoor());
            }
        }

        void GenerateTriggerAreaForDoor()
        {
            var triggerArea = new GameObject();
            triggerArea.name = "TriggerArea";
            triggerArea.transform.parent = this.transform;
            triggerArea.transform.localScale = new Vector3(10f, 1f, 1f);
            triggerArea.transform.rotation = transform.rotation;
            triggerArea.transform.position = transform.position;
            triggerArea.AddComponent<BoxCollider>();
            triggerArea.GetComponent<BoxCollider>().isTrigger = true;
            triggerArea.AddComponent<DoorTriggerBehavior>();
            _doorTriggerArea = triggerArea;
        }

        //open door/window/ventilationShaft Animation, currently placeholder
        IEnumerator OpenDoor()
        {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
            yield return new WaitForSeconds(2);
            GetComponent<MeshRenderer>().enabled = true;
            GetComponent<BoxCollider>().enabled = true;
        }
    }
}
