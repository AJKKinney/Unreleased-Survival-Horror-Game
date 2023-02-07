using UnityEditor;
using UnityEngine;

namespace AustenKinney.AudioSystem
{
    [RequireComponent(typeof(BoxCollider))]
    [System.Serializable]
    public class MusicZone : MonoBehaviour
    {
        [SerializeField] private bool adaptTrack = false;
        [SerializeField] private int targetTrack;
        [SerializeField] private int clip;
        [SerializeField] private float transitionLength = 1f;

        private MusicZoneMaster master;

        #region Getters & Setters

        public bool AdaptTrack { get { return adaptTrack; } set { adaptTrack = value; } }
        public int TargetTrack { get { return targetTrack; } set { targetTrack = value; } }
        public int Clip { get { return clip; } set { clip = value; } }
        public float TransitionLength { get { return transitionLength; } set { transitionLength = value; } }
        public MusicZoneMaster Master { get { return master; } set { master = value; } }

        #endregion

        private void Awake()
        {
            if (transform.parent.TryGetComponent<MusicZoneMaster>(out MusicZoneMaster zoneMaster))
            {
                master = zoneMaster;
            }
            else
            {
                Debug.LogWarning("Music Zone lacks a MusicZoneMaster component on its parent GameObject.");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Push Box") && other.CompareTag("Player"))
            {

                if (adaptTrack == true)
                {
                    master.AdaptTrack(targetTrack, clip, transitionLength);
                }

                master.CurrentVolumes.Add(this);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Push Box") && other.CompareTag("Player"))
            {
                master.FadeOutTrack(targetTrack, transitionLength);
            }

            master.CurrentVolumes.Remove(this);
        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MusicZone))]
    [CanEditMultipleObjects]
    public class MusicZoneScriptEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            var script = target as MusicZone;

            if (script.transform.parent.TryGetComponent<MusicZoneMaster>(out MusicZoneMaster zoneMaster))
            {
                script.Master = zoneMaster;
            }

            serializedObject.FindProperty("adaptTrack").boolValue = GUILayout.Toggle(script.AdaptTrack, "Adapt Track");

            if (script.AdaptTrack == true)
            {
                serializedObject.FindProperty("targetTrack").intValue = EditorGUILayout.IntField("Track", script.TargetTrack);


                EditorGUILayout.BeginHorizontal();

                script.Clip = EditorGUILayout.IntField("clip", script.Clip);

                if (script.Master != null && script.Master.Song != null && script.TargetTrack < script.Master.Song.Tracks.Count && script.Clip < script.Master.Song.Tracks[script.TargetTrack].Clips.Length)
                {
                    EditorGUILayout.LabelField(script.Master.Song.Tracks[script.TargetTrack].Clips[script.Clip].name);
                }
                else
                {
                    EditorGUILayout.LabelField("");
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Transition Length");
            serializedObject.FindProperty("transitionLength").floatValue = EditorGUILayout.Slider(script.TransitionLength, 0, 5);

            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}
