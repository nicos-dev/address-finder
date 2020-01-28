using System;
using System.Windows.Threading;

namespace FormValidator
{
#region License
/*
**************************************************************
*  Author: Rick Strahl 
*          © West Wind Technologies, 2017
*          http://www.west-wind.com/
* 
* Created: 07/3/2017
* 
* The above copyright notice and this permission notice shall be
* included in all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
* EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
* OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
* NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
* HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
* WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
* FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
* OTHER DEALINGS IN THE SOFTWARE.
**************************************************************
*/
#endregion

public class DebounceDispatcher
{

    private DispatcherTimer timer;
    private DateTime timerStarted { get; set; } = DateTime.UtcNow.AddYears(-1);

    public void Debounce(int interval, Action<object> action,
    object param = null,
    DispatcherPriority priority = DispatcherPriority.ApplicationIdle,
    Dispatcher disp = null)
    {
        timer?.Stop();
        timer = null;

        if (disp == null)
            disp = Dispatcher.CurrentDispatcher;

        timer = new DispatcherTimer(TimeSpan.FromMilliseconds(interval), priority, (s, e) =>
        {
            if (timer == null)
                return;

            timer?.Stop();
            timer = null;
            action.Invoke(param);
        }, disp);

        timer.Start();
    }

    public void Throttle(int interval, 
        Action<object> action,
        object param = null,
        DispatcherPriority priority = DispatcherPriority.ApplicationIdle,
        Dispatcher disp = null)
    {
        timer?.Stop();
        timer = null;

        if (disp == null)
            disp = Dispatcher.CurrentDispatcher;

        var curTime = DateTime.UtcNow;
        
        if (curTime.Subtract(timerStarted).TotalMilliseconds < interval)
            interval -= (int)curTime.Subtract(timerStarted).TotalMilliseconds;

        timer = new DispatcherTimer(TimeSpan.FromMilliseconds(interval), priority, (s, e) =>
        {
            if (timer == null)
                return;

            timer?.Stop();
            timer = null;
            action.Invoke(param);
        }, disp);

        timer.Start();
        timerStarted = curTime;
    }

}
}
 