using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerService.Models
{
    [Serializable]
    public class BugReport
    {
        public string Title;

        [Prompt("Enter a description for your report")]
        public string Description;

        [Prompt("What is your firstname?")]
        public string FirstName;

        [Describe("Surname")]
        public string LastName;

        [Prompt("What is the best date and time for a callback?")]
        public DateTime? BestTimeToCall;

        [Pattern(@"/^\s*(?:\+?(\d{1,3}))?[- (]*(\d{3})[- )]*(\d{3})[- ]*(\d{4})(?: *[x/#]{1}(\d+))?\s*$/")]
        public string PhoneNumber;

        [Prompt("Please list the bug areas that describe your issue. {||}")]
        public List<BugType> Bug;

        public Reproducibility Reproduce;

        public static IForm<BugReport> BuildForm()
        {
            return new FormBuilder<BugReport>().Message("Please fill out a bug.").Build();
        }
    }

    public enum BugType
    {
        Security = 1,
        Crash = 2,
        Power = 3,
        Performance = 4,
        Usability = 5,
        SeriousBug = 6,
        Other = 7
    }

    public enum Reproducibility
    {
        Always = 1,
        SomeTime = 2,
        Rearly = 3,
        Unable = 4
    }

}