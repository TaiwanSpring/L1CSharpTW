using System;
using System.Collections.Generic;

/// <summary>
///                            License
/// THE WORK (AS DEFINED BELOW) IS PROVIDED UNDER THE TERMS OF THIS  
/// CREATIVE COMMONS PUBLIC LICENSE ("CCPL" OR "LICENSE"). 
/// THE WORK IS PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW.  
/// ANY USE OF THE WORK OTHER THAN AS AUTHORIZED UNDER THIS LICENSE OR  
/// COPYRIGHT LAW IS PROHIBITED.
/// 
/// BY EXERCISING ANY RIGHTS TO THE WORK PROVIDED HERE, YOU ACCEPT AND  
/// AGREE TO BE BOUND BY THE TERMS OF THIS LICENSE. TO THE EXTENT THIS LICENSE  
/// MAY BE CONSIDERED TO BE A CONTRACT, THE LICENSOR GRANTS YOU THE RIGHTS CONTAINED 
/// HERE IN CONSIDERATION OF YOUR ACCEPTANCE OF SUCH TERMS AND CONDITIONS.
/// 
/// </summary>
namespace LineageServer.Server.Model
{

    using ListFactory = LineageServer.Utils.ListFactory;

    public class L1ExcludingList
    {

        private IList<string> _nameList = ListFactory.newList();

        public virtual void add(string name)
        {
            _nameList.Add(name);
        }

        /// <summary>
        /// 指定した名前のキャラクターを遮断リストから削除する
        /// </summary>
        /// <param name="name">
        ///            対象のキャラクター名 </param>
        /// <returns> 実際に削除された、クライアントの遮断リスト上のキャラクター名。 指定した名前がリストに見つからなかった場合はnullを返す。 </returns>
        public virtual string remove(string name)
        {
            foreach (string each in _nameList)
            {
                if (each == name)
                {
                    _nameList.Remove(each);
                    return each;
                }
            }
            return null;
        }

        /// <summary>
        /// 指定した名前のキャラクターを遮断している場合trueを返す
        /// </summary>
        public virtual bool contains(string name)
        {
            foreach (string each in _nameList)
            {
                if (each == name)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 遮断リストが上限の16名に達しているかを返す
        /// </summary>
        public virtual bool Full
        {
            get
            {
                return (_nameList.Count >= 16) ? true : false;
            }
        }
    }

}