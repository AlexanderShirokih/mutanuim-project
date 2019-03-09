using UnityEngine;
using System;

namespace Mutanium
{
    /// <summary>
    /// Представляет внутриигровую систему датирования начиная с новейшей эры (NE).
    /// Минимальная точность - минута. Одна секунда реального времени - 2 минуты виртуального. Один виртуальный год - 30 дней
    /// </summary>
    /// 
    public class Date
    {
        private const int MINUTES_IN_DAY = 60 * 24;
        private const int MINUTES_IN_YEAR = 365 * MINUTES_IN_DAY;
        private const float SCALE_TO_REALTIME = 20f;

        public Date()
        {
            CurrentTime = GameTime;
        }

        public Date(int min, int hour, int days, int year)
        {
            CurrentTime = min + 60f * (hour + 24f * (days + year * 365f));
        }



        /// <summary>
        /// Возвращает текущее внутриигровое время в минутах с начала эпохи
        /// </summary>
        /// <value>The game time.</value>
        public static float GameTime => GameSave.Instance.GameTime;

        /// <summary>
        /// Возвращает внутриигровое время в минутах
        /// </summary>
        /// <value>The current time.</value>
        public float CurrentTime { get; } = 0;

        /// <summary>
        /// Возвращает количество минут прошедших с текущей даты до настоящего времени
        /// </summary>
        /// <value>The days.</value>
        public int MinutesFromNow() => (int)(GameTime - CurrentTime);

        public string ToBasicString()
        {
            int m = (int)CurrentTime;
            int y = m / MINUTES_IN_YEAR;
            m -= y * MINUTES_IN_YEAR;
            int d = m / MINUTES_IN_DAY;
            m -= d * MINUTES_IN_DAY;
            int h = m / 60;
            m -= h * 60;
            return $"{h}:{m} D {d}, Y 000{ y }NC";
        }
        /// <summary>
        /// Вызывается в каждом кадре для обновления внутриигрового времени
        /// </summary>
        public static void UpdateTime()
        {
            GameSave.Current.GameTime += Time.deltaTime * SCALE_TO_REALTIME;
        }
    }
}
