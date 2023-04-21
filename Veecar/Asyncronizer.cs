using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Veecar
{
    internal static class Asyncronizer
    {
        public static Task<T> then<T>(this Task<T> task, Action<T> callback)
        {
            task.ContinueWith((it) => callback(it.Result), TaskContinuationOptions.OnlyOnRanToCompletion);
            return task;
        }
        public static Task Catch(this Task task, Action<Error> callback)
        {
            return Catch<object>((Task<object>)task, (err, it) => callback(err));
        }
        public static Task<T> Catch<T>(this Task<T> task, Func<Error, T?, T> callback)
        {
            var cancelated = task.ContinueWith<T>((it) => callback.Invoke("Cancelado", it.Result), TaskContinuationOptions.OnlyOnCanceled);
            var faulted = task.ContinueWith<T>((it) => callback.Invoke(it.Exception, default), TaskContinuationOptions.OnlyOnFaulted);
            return Task.WhenAny(task, cancelated, faulted).
                ContinueWith(t => t.Result == task? task.Result : t.Result.Result);
        }
        public static Task<T> Catch<T>(this Task<T> task, Action<Error, T> callback) => task.Catch<T>((err, it) => { callback?.Invoke(err, it); return it; });
        public static T Await<T>(this Task<T> task) { task.Wait(); return task.Result; }
        public static T Await<T>(this Task<T> task, CancellationToken token) { task.Wait(token); return task.Result; }
    }
    internal class Error
    {
        public string Message { get; internal set; }

        private Exception _excepcion = null;

        public override string ToString() => _excepcion?.Message??this.Message;

        public static implicit operator Error(Exception ex)
        {
            return new Error { _excepcion = ex };
        }
        public static implicit operator Exception(Error err)
        {
            return err._excepcion = new Exception(err.Message);
        }
        public static implicit operator bool(Error err)
        {
            return err.Message?.Length > 0;
        }
        public static implicit operator Error(string err)
        {
            return new() { Message = err };
        }
    }
}
