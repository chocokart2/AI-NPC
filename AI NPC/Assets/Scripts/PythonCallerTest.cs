using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class PythonCallerTest : MonoBehaviour
{
    public const bool I_HAVE_PYTHON_EXE = true;

    private System.Diagnostics.Process pythonProcess;
    private System.IO.StreamWriter pythonInput;
    private System.IO.StreamReader pythonOutput;
    private System.Threading.CancellationTokenSource cancellationTokenSource;



    public static string GetPath(string fileName)
        => $"Assets/PythonFiles/{fileName}.py";


    // Start is called before the first frame update
    void Start()
    {
        StartPythonProcess();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SendInputToPython(1, 2); // ���� �Է�
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            FinishPython(); // ���μ��� ����
        }
    }

    void OnDestroy()
    {
        FinishPython();
    }

    // Python ���μ����� ����
    private void StartPythonProcess()
    {
        if (pythonProcess != null)
        {
            Debug.LogWarning("Python process is already running.");
            return;
        }

        pythonProcess = new System.Diagnostics.Process()
        {
            StartInfo = new System.Diagnostics.ProcessStartInfo()
            {
                FileName = "python", // �̰� ���� ���α׷��̳�?
                Arguments = GetPath("PythonAsyncTest"), // ���̽� ���� �̸�
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            }
        };

        pythonProcess.Start();
        pythonInput = pythonProcess.StandardInput;
        pythonOutput = pythonProcess.StandardOutput;
        cancellationTokenSource = new System.Threading.CancellationTokenSource();
        Task.Run(() => M_ReadPythonOutputAsync(cancellationTokenSource.Token));
    }

    // Python ���μ������� ��� �б� (�񵿱�)
    private async Task M_ReadPythonOutputAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested && !pythonProcess.HasExited)
        {
            try
            {
                string line = await pythonOutput.ReadLineAsync();
                if (!string.IsNullOrEmpty(line))
                {
                    Debug.Log($"Python Output: {line}");
                }
            }
            catch
            {
                break;
            }
        }
    }

    public void SendInputToPython(int num1, int num2)
    {
        if (pythonInput == null || pythonProcess.HasExited)
        {
            Debug.LogError("Python process is not running.");
            return;
        }

        string input = $"{num1} {num2}";
        pythonInput.WriteLine(input);
        pythonInput.Flush();
        Debug.Log($"Sent to Python: {input}");
    }

    // Python ���μ��� ����
    public void FinishPython()
    {
        if (pythonProcess == null)
        {
            Debug.LogWarning("Python process is not running.");
            return;
        }

        cancellationTokenSource.Cancel(); // ��� �б� �۾� ���

        // Python ���μ��� ����
        pythonInput?.Close();
        pythonOutput?.Close();
        pythonProcess.Kill();
        pythonProcess.Dispose();
        pythonProcess = null;

        Debug.Log("Python process has been terminated.");
    }

}
