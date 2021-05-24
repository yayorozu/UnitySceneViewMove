using UnityEditor;
using UnityEngine;

namespace Yorozu.EditorTools
{
    [InitializeOnLoad]
    internal static class SceneViewMove
    {
        static SceneViewMove()
        {
            SceneView.duringSceneGui += SceneViewOnDuringSceneGui;
        }

        private static void SceneViewOnDuringSceneGui(SceneView obj)
        {
            var ev = Event.current;
            if (ev.type == EventType.KeyDown)
            {
                PressKey(ev.keyCode);
            }
        }

        /// <summary>
        /// 複数のKeyDownはとれない
        /// </summary>
        private static void PressKey(KeyCode keyCode)
        {
            if (EditorWindow.focusedWindow.GetType() != typeof(SceneView))
                return;

            var speed = 0.1f;
            var move = Vector3.zero;
            switch (keyCode)
            {
                case KeyCode.A:
                    move.x -= speed;
                    break;
                case KeyCode.D:
                    move.x += speed;
                    break;
                case KeyCode.W:
                    move.z += speed;
                    break;
                case KeyCode.S:
                    move.z -= speed;
                    break;
                case KeyCode.E:
                    move.y += speed;
                    break;
                case KeyCode.Q:
                    move.y -= speed;
                    break;
                default:
                    return;
            }

            if (move == Vector3.zero)
                return;

            var sceneView = SceneView.lastActiveSceneView;
            if (sceneView == null)
                return;

            var pivot = sceneView.pivot;
            var transform = sceneView.camera.transform;
            pivot += transform.TransformDirection(Vector3.forward) * move.z;
            pivot += transform.TransformDirection(Vector3.right) * move.x;
            pivot += transform.TransformDirection(Vector3.up) * move.y;
            sceneView.pivot = pivot;
            sceneView.Repaint();

            // ビープ音が鳴らない対応
            if (Event.current != null)
                Event.current.Use();
        }
    }
}
