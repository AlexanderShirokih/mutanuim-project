using System;
using Random = UnityEngine.Random;
using UnityEngine;

namespace Mutanium.Human
{
    public enum HumanStateName
    {
        IDLE, REST, PLAY, WALKING, DEFEND, CHASING, ATTACK, WORK, DYING, PLAYER
    };

    [Serializable]
    public class HumanInfo : UniqueElement
    {
        private static readonly ProbablyHumanStateName[] BABY_STATES =  {
                 new ProbablyHumanStateName(HumanStateName.IDLE, 0.20f),
                 new ProbablyHumanStateName(HumanStateName.REST, 0.15f),
                 new ProbablyHumanStateName(HumanStateName.PLAY, 0.65f)
                 };
        private static readonly ProbablyHumanStateName[] TEEN_STATES =  {
                 new ProbablyHumanStateName(HumanStateName.IDLE, 0.3f),
                 new ProbablyHumanStateName(HumanStateName.REST, 0.2f),
                 new ProbablyHumanStateName(HumanStateName.WALKING, 0.5f)
                 };
        private static readonly ProbablyHumanStateName[] ADULT_STATES =
        {
                 new ProbablyHumanStateName(HumanStateName.IDLE, 0.1f),
                 new ProbablyHumanStateName(HumanStateName.REST, 0.15f),
                 new ProbablyHumanStateName(HumanStateName.WALKING, 0.1f),
                 new ProbablyHumanStateName(HumanStateName.WORK, 0.65f)
                 };
        private static readonly ProbablyHumanStateName[] OLDER_STATES =
        {
                 new ProbablyHumanStateName(HumanStateName.IDLE, 0.2f),
                 new ProbablyHumanStateName(HumanStateName.REST, 0.3f),
                 new ProbablyHumanStateName(HumanStateName.WALKING, 0.5f)
                 };
        /// <summary>
        /// Позиция объекта в пространстве.
        /// </summary>
        public Vector3 Position
        {
            get
            {
                //Обновляем позицию из контроллера
                if (Controller != null)
                    _position = Controller.transform.position;
                return _position;
            }
            set
            {
                _position = value;
            }

        }
        [NonSerialized]
        private Vector3 _position;
        /// <summary>
        /// Поворот в углах Эйлера.
        /// </summary>
        public Vector3 EulerRotation
        {
            get
            {
                //Обновляем поворот из контроллера
                if (Controller != null)
                    _eulerRotation = Controller.transform.rotation.eulerAngles;
                return _eulerRotation;
            }
            set => _eulerRotation = value;
        }
        [NonSerialized]
        private Vector3 _eulerRotation;
        /// <summary>
        /// Дата рождения.
        /// </summary>
        /// <value>The birth date.</value>
        public Date BirthDate { get; set; }
        /// <summary>
        /// Пол персонажа.
        /// <value><c>true</c> если мужской, <c>false</c>  если женский.</value>
        public bool IsMen { get; set; }
        /// <summary>
        /// Возраст персонажа.
        /// </summary>
        /// <value>Возраст в годах.</value>
        public int Age { get; set; }
        /// <summary>
        /// Коэфициент усталости. [0..1]
        /// </summary>
        public float fatigue;
        /// <summary>
        /// Ссылка на привязанный дом.
        /// </summary>
        public ReferencedId<HouseInfo> AssignedHouse { get; set; }
        /// <summary>
        /// Ссылка на собственный персонаж
        /// </summary>
        public ReferencedId<HumanInfo> ReferencedId => new ReferencedId<HumanInfo>
        {
            RefId = Id
        };

        /// <summary>
        /// Контроллер текущего персонажа
        /// </summary>

        internal HumanController Controller { get; set; }

        public bool CanDefends() => Age >= 6;
        public bool CanAttack() => Age >= 14 && Age <= 65;

        private ProbablyHumanStateName[] AvailableStates
        {
            get
            {
                if (Age <= 5)
                {
                    return BABY_STATES;
                }
                if (Age <= 14)
                {
                    return TEEN_STATES;
                }
                if (Age <= 65)
                {
                    return ADULT_STATES;
                }
                return OLDER_STATES;
            }
        }

        public HumanStateName NextState
        {
            get
            {
                float rand = Random.value;
                float cumulative = 0f;
                HumanStateName last = HumanStateName.IDLE;

                foreach (var state in AvailableStates)
                {
                    cumulative += state.probability;
                    if (rand < cumulative)
                    {
                        last = state.state;
                        break;
                    }
                }

                return last;
            }
        }
    }
}
