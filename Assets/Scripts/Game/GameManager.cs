using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] private Canvas GameOverCanvas = default;

        [SerializeField] private GameObject BallPrefab = default;
        [SerializeField] private GameObject PlungerPrefab = default;
        [SerializeField] private GameObject PlungerSwitchPrefab = default;

        [SerializeField] private Text ScoreText = default;
        [SerializeField] private Text EndScoreText = default;
        [SerializeField] private LaunchHoleScript LaunchHoleLeft = default;
        [SerializeField] private LaunchHoleScript LaunchHoleRight = default;

        public int Score { get; private set; }
        public BallScript Ball { get; private set; }
        public PlungerScript Plunger { get; private set; }
        public PlungerSwitchScript PlungerSwitch { get; private set; }
        public int BallsLeft { get; private set; }
        public bool ReadyToPlay { get; private set; }
        public bool Tilt { get; set; }
        public BumperHitType OpenedHandle { get; private set; }

        public static event EventHandler OnBallUp;
        public static event EventHandler OnTilt;
        public void NewGame()
        {
            BallsLeft = 2;
            LoadBall();
            if (Plunger == null)
            {
                LoadPlunger();
            }
            else
            {
                ResetPlunger();
            }
        }

        public void BallOut()
        {
            BallsLeft -= 1;
            if (BallsLeft == -1)
            {
                GameOver();
            }
            else
            {
                LoadBall();
                ResetPlunger();
                ResetLaunchHoles();
            }
        }

        public void SetReady(bool ready)
        {
            ReadyToPlay = ready;
        }

        public void BallIn()
        {
            BallsLeft = Mathf.Min(3, BallsLeft + 1);
            OnBallUp?.Invoke(this, new EventArgs());
            UpdateBallCount();
            if (Ball == null)
            {
                LoadBall();
                ResetPlunger();
                ResetLaunchHoles();
            }
        }

        public void HitHandle(BumperHitType hitType)
        {
            var handles = FindObjectsOfType<HandleScript>();
            foreach(var handle in handles)
            {
                handle.Renderer.color = handle.BumperEnableType == hitType ? Color.green : Color.red;
            }
            var bumpers = FindObjectsOfType<BumperScript>();
            foreach(var bumper in bumpers)
            {
                if (bumper.hitType != BumperHitType.SimpleBumper)
                    bumper.Renderer.color = bumper.hitType == hitType ? bumper.startColor : BumperScript.DisableColor;
            }
            OpenedHandle = hitType;
        }

        public void HitBumper(BumperHitType hitType)
        {
            switch (hitType)
            {
                case BumperHitType.SimpleBumper:
                    Score += 100;
                    break;
                default:
                    if (OpenedHandle != hitType)
                    {
                        Score += 100;
                        break;
                    }
                    HitHandle(BumperHitType.SimpleBumper);
                    switch (hitType)
                    {
                        case BumperHitType.Points1:
                            Score += 1000;
                            break;
                        case BumperHitType.Points2:
                            Score += 2000;
                            break;
                        case BumperHitType.Points3:
                            Score += 3000;
                            break;
                        case BumperHitType.Points4:
                            Score += 4000;
                            break;
                        case BumperHitType.Points5:
                            Score += 5000;
                            break;
                        case BumperHitType.Points6:
                            Score += 6000;
                            break;
                        case BumperHitType.Points7:
                            Score += 7000;
                            break;
                        case BumperHitType.AdditionalBall:
                            BallIn();
                            break;
                        case BumperHitType.OpenLaunchHoles:
                            ResetLaunchHoles();
                            break;
                    }
                    break;
            }
        }

        private void GameOver()
        {
            SetReady(false);
            EndScoreText.text += Environment.NewLine + Mathf.Min(Score, 9999999).ToString("0000000");
            GameOverCanvas.gameObject.SetActive(true);
            Social.ReportScore(Score, GPGSIds.leaderboard_leaderboard_1_student_1, (b) => { });
        }

        private void ResetLaunchHoles()
        {
            LaunchHoleLeft.SetOpen(true);
            LaunchHoleRight.SetOpen(true);
        }

        private void ResetPlunger()
        {
            Plunger.AlreadyShot = false;
            Plunger.Plunging = false;
            PlungerSwitch.edgeTrigger.enabled = true;
            PlungerSwitch.polyCollider.enabled = false;
        }

        private void LoadBall()
        {
            UpdateBallCount();
            Ball = Instantiate(BallPrefab).GetComponent<BallScript>();
            Tilt = false;
        }

        private void UpdateBallCount()
        {
            var children = transform.Cast<Transform>().Reverse();
            foreach(var child in children)
            {
                child.gameObject.SetActive(false);
            }
            foreach(var child in children.Take(BallsLeft))
            {
                child.gameObject.SetActive(true);
            }
        }

        private void LoadPlunger()
        {
            Plunger = Instantiate(PlungerPrefab).transform.GetChild(0).GetComponent<PlungerScript>();
            PlungerSwitch = Instantiate(PlungerSwitchPrefab).GetComponent<PlungerSwitchScript>();
        }

        void Start()
        {
            Instance = this;
            NewGame();
            StartCoroutine(InitializeGyro());
        }

        IEnumerator InitializeGyro()
        {
            Input.gyro.enabled = true;
            yield return null;
            Debug.Log(Input.gyro.attitude);
        }

        void Update()
        {
            var gyroInput = SystemInfo.supportsGyroscope ? Input.gyro.gravity : Vector3.back;
            var gravity = Quaternion.Euler(-13, 0, 0) * gyroInput;
            if (Tilt)
            {
                Physics2D.gravity = Vector2.down * 4.41352f;
                ScoreText.text = "   TILT";
            }
            else
            {
                Physics2D.gravity = gravity * 4.41352f * 4.41352f;
                if ((Math.Abs(Physics2D.gravity.x) > 5 || Math.Abs(Physics2D.gravity.y + 4.41352f) > 5) && PlungerSwitch && PlungerSwitch.polyCollider && PlungerSwitch.polyCollider.enabled)
                {
                    Tilt = true;
                    OnTilt?.Invoke(this, new EventArgs());
                    Ball.AudioSource.clip = Ball.TiltSound;
                    Ball.AudioSource.PlayOneShot(Ball.TiltSound, 0.2f);
                }
                ScoreText.text = Mathf.Min(Score, 9999999).ToString("0000000");
            }
            
        }
    }
}
