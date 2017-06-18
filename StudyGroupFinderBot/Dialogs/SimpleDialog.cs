using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Graph;
using StudyGroupFinder;
using Util;

namespace StudyGroupFinderBot.Dialogs
{
    [Serializable]
    public class SimpleDialog : IDialog
    {
        private static Digraph<Student> digraph = StudentGraphGenerator.Sample();
        private static Student student = new Student("Aya1", "Fransk") { SeeksGroup = true, Attributes = new HashSet<string> { "P", "G", "R", "Q", "K" }, StudyAttributes = new HashSet<string> { "Ø", "A", "S", "I" } };

        public async Task StartAsync(IDialogContext context)
        {
            digraph.AddNode(new Node<Student>(student));

            foreach (string neighbor in new string[] { "Naja", "Luna", "Lea" })
            {
                digraph.AddEdge(student.Name, neighbor);
            }

            await context.PostAsync($"Velkommen til Studiegruppefinderen, {student.Name}");
            
            context.Wait(ActivityReceivedAsync);
        }

        private async Task ActivityReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            if (activity.Text.ToLower().Contains("hvor mange studerende"))
            {
                await context.PostAsync($"Der er {digraph.Nodes.Count} studerende.");
            }
            else if (activity.Text.ToLower().Contains("hvilke studerende"))
            {
                await context.PostAsync($"Følgende studerende er tilmeldt systemet:");
                await context.PostAsync(digraph.Nodes.Keys.ToSeparatedString(", "));
            }
            else if (activity.Text.ToLower().Contains("find nærmeste"))
            {
                await context.PostAsync($"Nærmeste studerende med samme fag: {digraph.FindPath(student.Name, s => s.Study == student.Study)}.");
                await context.PostAsync($"Nærmeste studerende med samme fag, som søger studiegruppe: {digraph.FindPath(student.Name, s => s.Study == student.Study && s.SeeksGroup)}.");
                await context.PostAsync($"Nærmeste studerende med samme fag, som søger studiegruppe + har mindst én studierelevant egenskab tilfælles: {digraph.FindPath(student.Name, s => s.SeeksGroup && digraph[student.Name].Data.Study == s.Study && (digraph[student.Name].Data.StudyAttributes.Intersect(s.StudyAttributes)).Count() > 0)}.");
                await context.PostAsync($"Nærmeste studerende  med mindst én egenskab tilfælles: {digraph.FindPath(student.Name, s => (digraph[student.Name].Data.Attributes.Intersect(s.Attributes)).Count() > 0)}.");
                await context.PostAsync($"Nærmeste studerende med mindst tre egenskaber tilfælles: {digraph.FindPath(student.Name, s => (digraph[student.Name].Data.Attributes.Intersect(s.Attributes)).Count() > 2)}.");
                await context.PostAsync($"Nærmeste studerende med egenskaben 'A': {digraph.FindPath(student.Name, s => s.Attributes.Contains("A"))}.");
            }
            else
            {
                await context.PostAsync($"Jeg kan fortælle hvilke og hvor mange studerende, der er tilmeldt systemet, samt finde den nærmeste studerende ud fra bestemte egenskaber.");
            }

            context.Wait(ActivityReceivedAsync);
        }
    }
}