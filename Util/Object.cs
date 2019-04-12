using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    public static class ObjectExtention
    {
        /// <summary>
        /// 合并对象属性
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="target"></param>
        /// <returns>返回的对象包含两对象的属性</returns>
        public static dynamic Merge(this object obj, object target)
        {
            dynamic expando = new ExpandoObject();
            var dic = (IDictionary<string, object>)expando;
            object value = null;
            PropertyDescriptorCollection objProps = TypeDescriptor.GetProperties(obj);
            PropertyDescriptorCollection targetProps = TypeDescriptor.GetProperties(target);
            for (int i = 0; i < objProps.Count; i++) {
                var p = objProps[i];
                if (dic.TryGetValue(p.Name, out value))
                    dic[p.Name] = p.GetValue(obj);
                else
                    dic.Add(p.Name, p.GetValue(obj));
            }

            for (int i = 0; i < targetProps.Count; i++) {
                var p = targetProps[i];
                if (dic.TryGetValue(p.Name, out value))
                    dic[p.Name] = p.GetValue(target);
                else
                    dic.Add(p.Name, p.GetValue(target));
            }
            return expando;
        }

    }
}
