using FileToEmailLinker.Models.Entities;

namespace FileToEmailLinker.Models.InputModels.Schedulations
{
    public class MonthlyScheduleInputModel
    {
        public bool One { get; set; }
        public bool Two { get; set; }
        public bool Three { get; set; }
        public bool Four { get; set; }
        public bool Five { get; set; }
        public bool Six { get; set; }
        public bool Seven { get; set; }
        public bool Eight { get; set; }
        public bool Nine { get; set; }
        public bool Ten { get; set; }
        public bool Eleven { get; set; }
        public bool Twelve { get; set; }
        public bool Thirteen { get; set; }
        public bool Fourteen { get; set; }
        public bool Fifteen { get; set; }
        public bool Sixteen { get; set; }
        public bool Seventeen { get; set; }
        public bool Eighteen { get; set; }
        public bool Nineteen { get; set; }
        public bool Twenty { get; set; }
        public bool Twentyone { get; set; }
        public bool Twentytwo { get; set; }
        public bool Twentythree { get; set; }
        public bool Twentyfour { get; set; }
        public bool Twentyfive { get; set; }
        public bool Twentysix { get; set; }
        public bool Twentyseven { get; set; }
        public bool Twentyeight { get; set; }
        public bool Twentynine { get; set; }
        public bool Thirty { get; set; }
        public bool Thirtyone { get; set; }
        public bool January { get; set; }
        public bool February { get; set; }
        public bool March { get; set; }
        public bool April { get; set; }
        public bool May { get; set; }
        public bool June { get; set; }
        public bool July { get; set; }
        public bool August { get; set; }
        public bool September { get; set; }
        public bool October { get; set; }
        public bool November { get; set; }
        public bool December { get; set; }

        public static MonthlyScheduleInputModel FromEntity(MonthlySchedulation monthlySchedulation)
        {
            MonthlyScheduleInputModel model = new MonthlyScheduleInputModel();
            model.One = monthlySchedulation.One;
            model.Two = monthlySchedulation.Two;
            model.Three = monthlySchedulation.Three;
            model.Four = monthlySchedulation.Four;
            model.Five = monthlySchedulation.Five;
            model.Six = monthlySchedulation.Six;
            model.Seven = monthlySchedulation.Seven;
            model.Eight = monthlySchedulation.Eight;
            model.Nine = monthlySchedulation.Nine;
            model.Ten = monthlySchedulation.Ten;
            model.Eleven = monthlySchedulation.Eleven;
            model.Twelve = monthlySchedulation.Twelve;
            model.Thirteen = monthlySchedulation.Thirteen;
            model.Fourteen = monthlySchedulation.Fourteen;
            model.Fifteen = monthlySchedulation.Fifteen;
            model.Sixteen = monthlySchedulation.Sixteen;
            model.Seventeen = monthlySchedulation.Seventeen;
            model.Eighteen = monthlySchedulation.Eighteen;
            model.Nineteen = monthlySchedulation.Nineteen;
            model.Twenty = monthlySchedulation.Twenty;
            model.Twentyone = monthlySchedulation.Twentyone;
            model.Twentytwo = monthlySchedulation.Twentytwo;
            model.Twentythree = monthlySchedulation.Twentythree;
            model.Twentyfour = monthlySchedulation.Twentyfour;
            model.Twentyfive = monthlySchedulation.Twentyfive;
            model.Twentysix = monthlySchedulation.Twentysix;
            model.Twentyseven = monthlySchedulation.Twentyseven;
            model.Twentyeight = monthlySchedulation.Twentyeight;
            model.Twentynine = monthlySchedulation.Twentynine;
            model.Thirty = monthlySchedulation.Thirty;
            model.Thirtyone = monthlySchedulation.Thirtyone;
            model.January = monthlySchedulation.January;
            model.February = monthlySchedulation.February;
            model.March = monthlySchedulation.March;
            model.April = monthlySchedulation.April;
            model.May = monthlySchedulation.May;
            model.June = monthlySchedulation.June;
            model.July = monthlySchedulation.July;
            model.August = monthlySchedulation.August;
            model.September = monthlySchedulation.September;
            model.October = monthlySchedulation.October;
            model.November = monthlySchedulation.November;
            model.December = monthlySchedulation.December;

            return model;
        }
    }
}
