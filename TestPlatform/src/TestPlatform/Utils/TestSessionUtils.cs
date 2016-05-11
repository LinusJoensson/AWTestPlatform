using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestPlatform.Models;

namespace TestPlatform.Utils
{
    public class TestSessionUtils
    {
        internal static int GetScore(TestSession ts, Answer[] _answers, Question[] _questions)
        {
            // Listan correctAnswers motsvarar antalet poäng (1 max per fråga) på provet
            // Int i listan motsvarar antalet rätta svar per fråga
            //correctAnswerCount += _answers.Where(o=>o.QuestionId == question.QuestionId)
            //    .Count(o => o.IsCorrect == true);

            // Initierar variabler
            var correctSelectedAnswerList = new List<int>();
            var maxTestScore = new List<int>();
            var testScore = new List<int>();
            int correctQuestionAnswerCount = 0;
            int correctSelectedAnswerCount = 0;

            // Facit-del
            #region Facit

            // Tar ut alla frågor relaterade till testsessionen
            var testQuestions = _questions.Where(o => o.TestId == ts.TestId).ToList();

            // Kollar varje fråga i testsessionen och se antalet rätta svar
            foreach (var question in testQuestions)
            {
                //Beräknar antalet rätta svar per fråga
                correctQuestionAnswerCount = question.Answers.Count(o => o.IsCorrect == true);
                maxTestScore.Add(correctQuestionAnswerCount);
            }
            #endregion
            #region User Answers
            // Loopar igenom frågorna för nuvarande test
            foreach (var questionResult in ts.QuestionResults)
            {
                // Splitta användarsvar till en array
                var selectedAnswers = questionResult.SelectedAnswers.Split(',');

                //Kolla antal korrekta svar per fråga
                foreach (var answer in selectedAnswers)
                {
                    var answerId = Convert.ToInt32(answer);
                    correctSelectedAnswerCount += _answers.Where(o => o.Id == answerId)
                        .Count(o => o.IsCorrect == true);
                }
                correctSelectedAnswerList.Add(correctSelectedAnswerCount);
                correctSelectedAnswerCount = 0;
            }
            #endregion

            #region Rättning
            for (int i = 0; i < maxTestScore.Count; i++)
            {
                if (maxTestScore[i] == correctSelectedAnswerList[i])
                {
                    System.Diagnostics.Debug.WriteLine(correctSelectedAnswerList[i]);
                    System.Diagnostics.Debug.WriteLine(maxTestScore[i]);
                    testScore.Add(correctSelectedAnswerList[i]);
                }
            }

            System.Diagnostics.Debug.WriteLine(testScore.Count());
            #endregion
            return testScore.Count();

            // Svarslistan skapas i TestPlatformRepository UpdateSessionAnswers, Kommaseparerad lista

            // Kolla allas svar och se om de är rätt/fel. Slå ihop antal rätt.
            // Flervalsfrågor, endast de korrekta svaren måste vara icheckade.
            // Textsvar, manuell rättning med boolean som false ursprungligen

            // Vi tar in tesstsessionen, kollar de valda svaren per fråga.
            // Kollar 
        }
    }
}
