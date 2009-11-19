using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.NameManagePlugin
{
	class User
	{
		public enum UserState { 
			None,
			New,
			Update,
			Delete
		}

		public string Id;
		public string Name;
		public System.Drawing.Color Color;
		public DateTime LastCommentDate;
		public UserState State;

		public User() {
			Color = System.Drawing.Color.Transparent;
			State = UserState.New;
		}

		public User(string id)
		{
			Id = id;
			Color = System.Drawing.Color.Transparent;
			State = UserState.New;
		}

		public User(string id, string name, int color, long tick) {
			Id = id;
			Name = name;
			Color = System.Drawing.Color.FromArgb(color);
			LastCommentDate = new DateTime(tick);
			State = UserState.None;
		}
	}
}
