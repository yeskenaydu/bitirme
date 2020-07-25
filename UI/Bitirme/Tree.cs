using IronPython.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitirme
{
    public class Tree
    {
        public string AltUst(double ms1,double ms0, double ms2, double alt, double ust)
        {



            if (alt > 1.73)
            {
                if (ms2 <= 11)
                {
                    if (ms1 > 1.66)
                    {
                        if (ms0 > 3.29)
                        {
                            if (ust <= 2.11)
                                return "Ust";
                            else
                                return "Alt";
                        }
                        else
                        {
                            return "Alt";
                        }
                    }
                    else
                    {
                        if (ust <= 2.11)
                            return "Ust";
                        else
                            return "Alt";
                    }
                }
                else
                {
                    return "Ust";

                }
            }
            else
            {
                if(ms0<=3.29)
                {
                    if (ust > 2.11)
                        return "Alt";
                    else
                        return "Ust";
                }
                else
                {
                    if (ust > 2.11)
                        return "Alt";
                    else
                    {
                        if (ms1 > 1.66)
                            return "Alt";
                        else
                            return "Ust";
                    }
                }
            }

        }

        /*public string MacSonuc(double ms1, double ms0, double ms2, double alt, double ust)
        {
            var engine = Python.CreateEngine();

            var script = @"C:\Users\Baran\PycharmProjects\untitled\outputs\rules\rules.py";
            var source = engine.CreateScriptSourceFromFile(script);

            var argv = new List<double>();
            argv.Add(ms1);
            argv.Add(ms0);
            argv.Add(ms2);
            argv.Add(alt);
            argv.Add(ust);

            engine.GetSysModule().SetVariable("argv", argv);

            var eIO = engine.Runtime.IO;

            var errors = new MemoryStream();
            eIO.SetErrorOutput(errors, Encoding.Default);

            var results = new MemoryStream();
            eIO.SetOutput(results, Encoding.Default);

            var scope = engine.CreateScope();
            source.Execute(scope);

            string str(byte[] x) => Encoding.Default.GetString(x);

            return str(errors.ToArray());


        }
        public string MacSonuc(double ms1, double ms0, double ms2, double alt, double ust)
        {
            var psi = new ProcessStartInfo();
            psi.FileName = @"C:\Users\Baran\AppData\Local\Programs\Python\Python38\python.exe";
            var script = @"C:\Users\Baran\PycharmProjects\untitled\outputs\rules\rules.py";
            psi.Arguments = $"{script} {ms1} {ms0} {ms2} {alt} {ust} ";

            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            var errors = "";
            var results = "";
            using (Process process = Process.Start(psi))
            {
                errors = process.StandardError.ReadToEnd();
                results = process.StandardOutput.ReadToEnd();
            }
            return errors + " " + results;
        }
        */
        public string MacSonucu(double ms1,double ms0, double ms2)
        {
            if(ms2 > 3.1)
            {
                return "Ev";
            }
            else
            {
                if (ms0 < 4.09)
                {
                    if (ms1 > 2.2)
                    {
                        return "Deplasman";
                    }
                    else
                    {
                        return "Ev";
                    }
                }
                else
                    return "Deplasman";
            }
        }
    }
}
