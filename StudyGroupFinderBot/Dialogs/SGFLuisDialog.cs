using Graph;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using StudyGroupFinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Util;

namespace StudyGroupFinderBot.Dialogs
{
    [LuisModel("", "")]
    [Serializable]
    public class SGFLuisDialog : LuisDialog<object>
    {
        private static Digraph<Student> digraph = StudentGraphGenerator.Sample();
        private static Student currentStudent;
        private static Student newStudent;
        private static string[] newNeighbors;
        private int newStudentStep = -1;
        private string student = "";

        [LuisIntent("Restart")]
        public async Task Restart(IDialogContext context, LuisResult result)
        {
            PromptDialog.Confirm(context, RestartAsync, $"Are you sure you want to restart? All changes will be lost.");
        }

        private async Task RestartAsync(IDialogContext context, IAwaitable<bool> result)
        {
            if (await result)
            {
                digraph = StudentGraphGenerator.Sample();
                await context.PostAsync($"The system has been restarted.");
            }
            else
            {
                await context.PostAsync($"Restart was cancelled.");
            }

            context.Wait(MessageReceived);
        }

        [LuisIntent("StudentCount")]
        public async Task StudentCount(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"There are currently {digraph.Nodes.Count} students registered in the system.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("StudentList")]
        public async Task StudentList(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Registered students: { digraph.Nodes.Keys.ToSeparatedString(", ") }.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("StudentDetails")]
        public async Task StudentDetails(IDialogContext context, LuisResult result)
        {
            string student = "";
            EntityRecommendation rec;

            if (result.TryFindEntity("Student", out rec))
            {
                try
                {
                    student = rec.Entity.Substring(0, 1).ToUpper() + rec.Entity.Substring(1).ToLower();

                    if (digraph.Contains(student))
                    {
                        await context.PostAsync($"{ digraph[student].Data.GetDetails() }, Neighbors: [{ digraph[student].Neighbors.ToSeparatedString() }].");
                    }
                    else
                    {
                        await context.PostAsync("The system does not contain a student with the given name.");
                    }
                }
                catch (Exception ex)
                {
                    await context.PostAsync($"Whoops, something went wrong. Please try again. Details: { ex.Message }");
                }
            }
            else
            {
                await context.PostAsync("The student name was not recognized. Please try again.");
            }

            context.Wait(MessageReceived);
        }

        [LuisIntent("AddStudent")]
        public async Task AddStudent(IDialogContext context, LuisResult result)
        {
            newStudentStep = 0;
            await context.PostAsync("What is the student's name?");
            context.Wait(AddStudentAsync);
        }

        private async Task AddStudentAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            if (newStudentStep == 0)
            {
                // Name
                string newName = activity.Text.Substring(0, 1).ToUpper() + activity.Text.Substring(1).ToLower();

                if (digraph.Contains(newName))
                {
                    newName = StudentGraphGenerator.GenerateUniqueName(digraph, newName);
                    await context.PostAsync($"Because the system already contains a student with the given name, it has been changed to { newName }.");
                }

                newStudent = new Student(newName);
                newStudentStep = 1;
                await context.PostAsync($"What subject does { newStudent } study?");
                context.Wait(AddStudentAsync);
            }
            else if (newStudentStep == 1)
            {
                // Study
                newStudent.Study = activity.Text.Substring(0, 1).ToUpper() + activity.Text.Substring(1);
                newStudentStep = 2;
                PromptDialog.Confirm(context, StudyGroupAsync, $"Does { newStudent } seek a study group?");
            }
            else if (newStudentStep == 2)
            {
                // SeeksGroup (has already been handled in separate method - this code should never be reached)
                newStudentStep = 3;
                context.Wait(AddStudentAsync);
            }
            else if (newStudentStep == 3)
            {
                // Attributes
                if (activity.Text == "-") activity.Text = "";
                newStudent.Attributes = new HashSet<string>(activity.Text.ToUpper().Split(' '));
                newStudentStep = 4;
                await context.PostAsync($"Enter { newStudent }'s study attributes separated by spaces. Type '-' if there are none.");
                context.Wait(AddStudentAsync);
            }
            else if (newStudentStep == 4)
            {
                // StudyAttributes
                if (activity.Text == "-") activity.Text = "";
                newStudent.StudyAttributes = new HashSet<string>(activity.Text.ToUpper().Split(' '));
                newStudentStep = 5;
                await context.PostAsync($"Enter { newStudent }'s neighbors separated by spaces. Type '-' if there are none.");
                context.Wait(AddStudentAsync);
            }
            else if (newStudentStep == 5)
            {
                // Neighbors
                string[] rawNeighbors;

                if (activity.Text == "-")
                {
                    rawNeighbors = new string[] { };
                }
                else
                {
                    rawNeighbors = activity.Text.Split(' ');
                }
                
                newNeighbors = new string[rawNeighbors.Length];

                for (int i = 0; i < rawNeighbors.Length; i++)
                {
                    newNeighbors[i] = rawNeighbors[i].Substring(0, 1).ToUpper() + rawNeighbors[i].Substring(1).ToLower();
                }

                newStudentStep = 0;
                await context.PostAsync($"The new student's details are: { newStudent.GetDetails() }, Neighbors: [{ newNeighbors.ToSeparatedString() }]");
                PromptDialog.Confirm(context, NewStudentAsync, $"Can you confirm the details?");
            }
            else
            {
                // Error
                await context.PostAsync($"I am a bit confused, sorry. Please try again.");
                context.Wait(MessageReceived);
            }
        }

        private async Task NewStudentAsync(IDialogContext context, IAwaitable<bool> result)
        {
            if (await result)
            {
                digraph.AddNode(new Node<Student>(newStudent));

                foreach (string neighbor in newNeighbors)
                {
                    digraph.AddEdge(newStudent.Name, neighbor);
                }

                await context.PostAsync($"{ newStudent.Name } was added to the system.");
            }
            else
            {
                await context.PostAsync($"The operation was cancelled. Type 'new student' to try again.");
            }

            context.Wait(MessageReceived);
        }

        private async Task StudyGroupAsync(IDialogContext context, IAwaitable<bool> result)
        {
            if (await result)
            {
                newStudent.SeeksGroup = true;
            }
            else
            {
                newStudent.SeeksGroup = true;
            }

            newStudentStep = 3;
            await context.PostAsync($"Enter { newStudent }'s attributes separated by spaces. Type '-' if there are none.");
            context.Wait(AddStudentAsync);
        }

        [LuisIntent("RemoveStudent")]
        public async Task RemoveStudent(IDialogContext context, LuisResult result)
        {
            EntityRecommendation rec;

            if (result.TryFindEntity("Student", out rec))
            {
                try
                {
                    student = rec.Entity.Substring(0, 1).ToUpper() + rec.Entity.Substring(1).ToLower();

                    if (digraph.Contains(student))
                    {
                        PromptDialog.Confirm(context, RemoveStudentAsync, $"Are you sure you want to delete { student }?");
                    }
                    else
                    {
                        await context.PostAsync("The system does not contain a student with the given name.");
                        context.Wait(MessageReceived);
                    }
                }
                catch (Exception ex)
                {
                    await context.PostAsync($"Whoops, something went wrong. Please try again. Details: { ex.Message }");
                    context.Wait(MessageReceived);
                }
            }
            else
            {
                await context.PostAsync("The student name was not recognized. Please try again.");
                context.Wait(MessageReceived);
            }
        }

        private async Task RemoveStudentAsync(IDialogContext context, IAwaitable<bool> result)
        {
            if (await result)
            {
                digraph.RemoveNode(student);
                await context.PostAsync($" { student } has been removed from the system.");
            }
            else
            {
                await context.PostAsync($"Removal of { student } was cancelled.");
            }

            context.Wait(MessageReceived);
        }

        [LuisIntent("Find")]
        public async Task Find(IDialogContext context, LuisResult result)
        {
            string student = "";
            EntityRecommendation rec;            

            try
            {
                if (currentStudent is null)
                {
                    await context.PostAsync("You must first select a student to search from. Type 'set current student to' followed by a name.");
                }
                else if (result.TryFindEntity("Student", out rec))
                {
                    student = rec.Entity.Substring(0, 1).ToUpper() + rec.Entity.Substring(1).ToLower();

                    if (digraph.Contains(student))
                    {
                        Path path = digraph.FindPath(currentStudent.Name, student);

                        if (path.Count > 0)
                        {
                            await context.PostAsync($"Path from { currentStudent } to { student }: { path }.");
                        }
                        else
                        {
                            await context.PostAsync($"There is no path between { currentStudent } and { student }.");
                        }
                    }
                    else
                    {
                        await context.PostAsync("The system does not contain a student with the given name.");
                    }
                }
                else if (result.Query.Contains("group"))
                {
                    string study = currentStudent.Study;

                    Path path = digraph.FindShortestPath(currentStudent.Name, s => s.SeeksGroup && s.Study == study);

                    if (path.Count > 0)
                    {
                        await context.PostAsync($"Path from { currentStudent } to closest study group prospect: { path }.");
                    }
                    else
                    {
                        await context.PostAsync($"Sorry, there is no path from { currentStudent } to a student who also studies { study } and seeks a study group.");
                    }
                }
                else if (result.Query.Contains("same study"))
                {
                    string study = currentStudent.Study;

                    Path path = digraph.FindShortestPath(currentStudent.Name, s => s.Study == study);

                    if (path.Count > 0)
                    {
                        await context.PostAsync($"Path from { currentStudent } to closest student who also studies { study }: { path }.");
                    }
                    else
                    {
                        await context.PostAsync($"There is no path from { currentStudent } to a student who also studies { study }.");
                    }
                }
                else
                {
                    await context.PostAsync("Sorry, I didn't understand. Please try again.");
                }
            }
            catch (Exception ex)
            {
                await context.PostAsync($"Whoops, something went wrong. Please try again. Details: { ex.Message }");
            }

            context.Wait(MessageReceived);
        }

        [LuisIntent("GetCurrent")]
        public async Task GetCurrent(IDialogContext context, LuisResult result)
        {
            if (currentStudent is null)
            {
                await context.PostAsync("No student is currently selected.");
            }
            else
            {
                await context.PostAsync($"The currently selected student is { currentStudent.Name }.");
            }

            
            context.Wait(MessageReceived);
        }

        [LuisIntent("SetCurrent")]
        public async Task SetCurrent(IDialogContext context, LuisResult result)
        {
            string student = "";
            EntityRecommendation rec;

            if (result.TryFindEntity("Student", out rec))
            {
                try
                {
                    student = rec.Entity.Substring(0, 1).ToUpper() + rec.Entity.Substring(1).ToLower();

                    if (digraph.Contains(student))
                    {
                        currentStudent = digraph[student].Data;
                        await context.PostAsync($"The current student has been set to { student }");
                    }
                    else
                    {
                        await context.PostAsync("The system does not contain a student with the given name.");
                    }
                }
                catch (Exception ex)
                {
                    await context.PostAsync($"Whoops, something went wrong. Please try again. Details: { ex.Message }");
                }
            }
            else
            {
                await context.PostAsync("The student name was not recognized. Please try again.");
            }

            context.Wait(MessageReceived);
        }

        [LuisIntent("ChangePathFinder")]
        public async Task ChangePathFinder(IDialogContext context, LuisResult result)
        {
            string pathFinder = "";
            EntityRecommendation rec;

            if (result.TryFindEntity("PathFinder", out rec))
            {
                try
                {
                    pathFinder = rec.Entity.ToLower();

                    if (pathFinder == "bfs")
                    {
                        digraph.PathFinder = new BFS<Student>();
                    }
                    else if (pathFinder == "dfs")
                    {
                        digraph.PathFinder = new DFS<Student>();
                    }

                    await context.PostAsync($"The pathfinder algorithm has been set to { digraph.PathFinder }");
                }
                catch (Exception)
                {
                    await context.PostAsync("Sorry, there is no pathfinder algorithm with the given name. Try BFS or DFS.");
                }
            }
            else
            {
                await context.PostAsync("Sorry, there is no pathfinder algorithm with the given name. Try BFS or DFS.");
            }
            
            context.Wait(MessageReceived);
        }

        [LuisIntent("DeepThought")]
        public async Task DeepThought(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("42");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Help")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Help is under construction.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Sorry, I did not understand. Please try again.");
            context.Wait(MessageReceived);
        }
    }
}