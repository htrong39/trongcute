using System;
using System.IO;
using System.Windows.Forms;

namespace List_am_nhac
{
    public partial class Form1 : Form
    { // Các biến toàn cục với mô tả
        private DoublyLinkedList songList = new DoublyLinkedList();      // Danh sách tất cả bài hát
        private DoublyLinkedList songHistory = new DoublyLinkedList();   // Lưu lịch sử phát nhạc
        private DoublyLinkedList favoriteSongs = new DoublyLinkedList(); // Danh sách bài hát yêu thích
        private Node currentSong;    // Bài hát đang phát
        private int currentIndex = 0;    // Vị trí bài hát hiện tại
        private Form suggestionForm; // Form hiển thị gợi ý bài hát
        private ListBox listBoxSuggestions;  // ListBox chứa danh sách gợi ý                     


        public Form1()
        {
            InitializeComponent();

            InitializeEventHandlers();
        }
        // Khởi tạo các sự kiện
        private void InitializeEventHandlers()
        {
            axWindowsMediaPlayer1.PlayStateChange += axWindowsMediaPlayer1_PlayStateChange;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
        }
        // Lớp Node đại diện cho một nút trong danh sách liên kết đôi
        public class Node
        {
            public string Data { get; set; }     // Đường dẫn file nhạc
            public Node Previous { get; set; }   // Con trỏ đến node trước
            public Node Next { get; set; }       // Con trỏ đến node sau

            public Node(string data)
            {
                Data = data;
                Previous = null;
                Next = null;
            }
        }

        // Lớp danh sách liên kết đôi tự định nghĩa
        public class DoublyLinkedList
        {
            public Node Head { get; private set; }  // Nút đầu danh sách
            public Node Tail { get; private set; }  // Nút cuối danh sách
            public int Count { get; private set; }  // Số lượng phần tử

            public DoublyLinkedList()
            {
                Head = null;
                Tail = null;
                Count = 0;
            }
            // Xóa node có dữ liệu tương ứng với data
            public void Remove(string data)
            {
                Node nodeToRemove = Find(data);
                if (nodeToRemove == null) return; // Không tìm thấy node để xóa

                // Xóa node khỏi danh sách liên kết đôi
                if (nodeToRemove.Previous != null)
                    nodeToRemove.Previous.Next = nodeToRemove.Next;
                else
                    Head = nodeToRemove.Next; // Cập nhật Head bên trong class

                if (nodeToRemove.Next != null)
                    nodeToRemove.Next.Previous = nodeToRemove.Previous;
                else
                    Tail = nodeToRemove.Previous; // Cập nhật Tail bên trong class

                Count--; // Giảm số lượng phần tử
            }


            // Thêm phần tử vào đầu danh sách (dùng cho lịch sử)
            public void AddFirst(string data)
            {
                Node newNode = new Node(data);
                if (Head == null)
                {
                    Head = Tail = newNode;
                }
                else
                {
                    newNode.Next = Head;
                    Head.Previous = newNode;
                    Head = newNode;
                }
                Count++;
            }

            // Thêm phần tử vào cuối danh sách (dùng cho danh sách chính)
            public void AddLast(string data)
            {
                Node newNode = new Node(data);
                if (Head == null)
                {
                    Head = Tail = newNode;
                }
                else
                {
                    newNode.Previous = Tail;
                    Tail.Next = newNode;
                    Tail = newNode;
                }
                Count++;
            }

            // Xóa và trả về phần tử đầu tiên (dùng cho lịch sử)
            public string RemoveFirst()
            {
                if (Head == null) return null;
                string data = Head.Data;
                Head = Head.Next;
                if (Head == null)
                    Tail = null;
                else
                    Head.Previous = null;
                Count--;
                return data;
            }

            // Xóa toàn bộ danh sách
            public void Clear()
            {
                Head = Tail = null;
                Count = 0;
            }

            // Tìm node theo đường dẫn file
            public Node Find(string data)
            {
                Node current = Head;
                while (current != null)
                {
                    if (current.Data == data)
                        return current;
                    current = current.Next;
                }
                return null;
            }

            // Lấy node tại vị trí chỉ định
            public Node GetNodeAt(int index)
            {
                if (index < 0 || index >= Count) return null;
                Node current = Head;
                for (int i = 0; i < index && current != null; i++)
                {
                    current = current.Next;
                }
                return current;
            }

            // Xáo trộn danh sách ngẫu nhiên
            public void Shuffle()
            {
                if (Count <= 1) return;

                Random rng = new Random();
                Node current = Head;
                int length = Count;

                for (int i = 0; i < length && current != null; i++)
                {
                    int swapIndex = rng.Next(0, length);
                    Node swapNode = GetNodeAt(swapIndex);
                    if (current != swapNode && swapNode != null)
                    {
                        string temp = current.Data;
                        current.Data = swapNode.Data;
                        swapNode.Data = temp;
                    }
                    current = current.Next;
                }
            }

            // Sắp xếp danh sách bằng thuật toán MergeSort
            public void MergeSort()
            {
                if (Count <= 1) return;
                Head = MergeSortList(Head);
                Tail = Head;
                while (Tail != null && Tail.Next != null)
                {
                    Tail = Tail.Next;
                }
            }

            // Hàm hỗ trợ MergeSort: chia danh sách
            private Node MergeSortList(Node head)
            {
                if (head == null || head.Next == null)
                    return head;

                Node slow = head;
                Node fast = head.Next;

                while (fast != null && fast.Next != null)
                {
                    slow = slow.Next;
                    fast = fast.Next.Next;
                }
                Node secondHalf = slow.Next;
                slow.Next = null;
                if (secondHalf != null)
                    secondHalf.Previous = null;

                Node left = MergeSortList(head);
                Node right = MergeSortList(secondHalf);
                return Merge(left, right);
            }

            // Hàm hỗ trợ MergeSort: hợp nhất danh sách
            private Node Merge(Node left, Node right)
            {
                if (left == null) return right;
                if (right == null) return left;

                Node result;
                if (string.Compare(left.Data, right.Data) <= 0)
                {
                    result = left;
                    result.Next = Merge(left.Next, right);
                    if (result.Next != null)
                        result.Next.Previous = result;
                }
                else
                {
                    result = right;
                    result.Next = Merge(left, right.Next);
                    if (result.Next != null)
                        result.Next.Previous = result;
                }
                result.Previous = null;
                return result;
            }
        }

        // --------------------- HÀM HỖ TRỢ ---------------------

        private void PlayCurrentSong()
        {
            if (currentSong != null)
            {
                axWindowsMediaPlayer1.URL = currentSong.Data;
                axWindowsMediaPlayer1.Ctlcontrols.play();
                listBox1.SelectedIndex = currentIndex;

                // Tạo chuỗi lịch sử: "đường dẫn|thời gian"
                string historyEntry = $"{currentSong.Data}|{DateTime.Now:HH:mm:ss dd/MM/yyyy}";

                // Kiểm tra bài hát gần nhất trong lịch sử
                if (songHistory.Head != null)
                {
                    string[] lastEntryParts = songHistory.Head.Data.Split('|');
                    if (lastEntryParts.Length > 0 && lastEntryParts[0] == currentSong.Data)
                    {
                        // Nếu bài hát hiện tại trùng với bài hát gần nhất trong lịch sử, không thêm lại
                        return;
                    }
                }

                // Nếu không trùng, thêm vào lịch sử
                songHistory.AddFirst(historyEntry);// sử dụng DoublyLinkedList để quản lý lịch sử 
            }
        }

        // Cập nhật giao diện ListBox từ danh sách bài hát
        private void UpdateListBox()
        {
            listBox1.Items.Clear();
            Node current = songList.Head;
            while (current != null)
            {
                listBox1.Items.Add(Path.GetFileName(current.Data));
                current = current.Next;
            }
        }
        // Phát bài hát tiếp theo trong danh sách
        private void PlayNextSong()
        {
            if (songList.Count == 0 || currentSong == null)
            {
                axWindowsMediaPlayer1.Ctlcontrols.stop();
                return;
            }

            int nextIndex = currentIndex + 1;
            if (nextIndex >= songList.Count)
            {

                axWindowsMediaPlayer1.Ctlcontrols.stop();
                currentSong = null;
                currentIndex = -1;
                listBox1.SelectedIndex = -1;
                return;
            }

            currentIndex = nextIndex;
            currentSong = songList.GetNodeAt(nextIndex);
            PlayCurrentSong();
        }
        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (e.newState == (int)WMPLib.WMPPlayState.wmppsMediaEnded)
            {
                if (songList.Count > 0)
                {
                    PlayNextSong();
                }
                else
                {
                    axWindowsMediaPlayer1.Ctlcontrols.stop();
                }
            }
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Mp3 files, mp4 files (*.mp3, *.mp4)|*.mp*",
                Multiselect = true,
                Title = "Chọn Nhạc"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                songList.Clear();
                foreach (var file in openFileDialog.FileNames)
                {
                    songList.AddLast(file);
                }
                UpdateListBox();
                if (songList.Head != null)
                {
                    currentSong = songList.Head;
                    currentIndex = 0;
                    PlayCurrentSong();
                }
            }
        }

        // Xáo trộn danh sách bài hát
        private void btnShuffle_Click(object sender, EventArgs e)
        {

            if (songList.Count > 0)
            {
                songList.Shuffle();
                UpdateListBox();
                currentSong = songList.Head;
                currentIndex = 0;
                PlayCurrentSong();
            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.close(); // Đóng Windows Media Player trước
            Application.Exit();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Form inputForm = new Form
            {
                Text = "Chọn ký tự đầu của bài hát",
            };

            FlowLayoutPanel panel = new FlowLayoutPanel { Dock = DockStyle.Fill };
            inputForm.Controls.Add(panel);

            char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            foreach (char letter in alphabet)
            {
                Button letterButton = new Button
                {
                    Text = letter.ToString(),
                    Width = 30,
                    Height = 30
                };
                letterButton.Click += (s, ev) =>
                {
                    inputForm.Close();
                    ShowSuggestedSongs(letter);
                };
                panel.Controls.Add(letterButton);
            }

            inputForm.ShowDialog();
        }

        // Hiển thị danh sách bài hát gợi ý theo chữ cái
        private void ShowSuggestedSongs(char letter)
        {
            DoublyLinkedList suggestions = new DoublyLinkedList();
            Node current = songList.Head;
            while (current != null)
            {
                string songName = Path.GetFileNameWithoutExtension(current.Data);
                if (!string.IsNullOrEmpty(songName) && char.ToUpper(songName[0]) == char.ToUpper(letter))
                {
                    suggestions.AddLast(current.Data);
                }
                current = current.Next;
            }

            if (suggestionForm != null)
            {
                suggestionForm.Close();
            }

            suggestionForm = new Form
            {
                Text = "Bài hát gợi ý",
            };

            listBoxSuggestions = new ListBox { Dock = DockStyle.Fill };
            current = suggestions.Head;
            while (current != null)
            {
                listBoxSuggestions.Items.Add(Path.GetFileName(current.Data));
                current = current.Next;
            }

            listBoxSuggestions.MouseDoubleClick += listBoxSuggestions_MouseDoubleClick;
            suggestionForm.Controls.Add(listBoxSuggestions);
            suggestionForm.ShowDialog();
        }

        // Xử lý khi double click vào bài hát gợi ý
        private void listBoxSuggestions_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBoxSuggestions.SelectedItem != null)
            {
                string selectedSongName = listBoxSuggestions.SelectedItem.ToString();
                Node temp = songList.Head;
                while (temp != null)
                {
                    if (Path.GetFileName(temp.Data) == selectedSongName)
                    {
                        currentSong = temp;
                        currentIndex = 0;
                        Node tempSong = songList.Head;
                        while (tempSong != null && tempSong != temp)
                        {
                            currentIndex++;
                            tempSong = tempSong.Next;
                        }
                        PlayCurrentSong();
                        suggestionForm?.Close();
                        break;
                    }
                    temp = temp.Next;
                }
            }
        }

        // Thêm bài hát vào danh sách yêu thích
        private void btnAddToFavorites_Click(object sender, EventArgs e)
        {
            if (currentSong != null)
            {
                string currentSongPath = currentSong.Data;
                if (favoriteSongs.Find(currentSongPath) == null)
                {
                    favoriteSongs.AddLast(currentSongPath);
                    MessageBox.Show("Đã thêm vào mục yêu thích!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Bài hát đã có trong mục yêu thích!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

        }

        // Hiển thị danh sách bài hát yêu thích và cho phép xóa từng bài hát
        private void btnShowFavorites_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem danh sách yêu thích có rỗng không, nếu rỗng thì hiển thị thông báo và thoát
            if (favoriteSongs.Count == 0)
            {
                MessageBox.Show("Danh sách yêu thích trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Tạo form mới để hiển thị danh sách yêu thích
            Form favoritesForm = new Form
            {
                Text = "Danh sách yêu thích",
                Width = 400,
                Height = 300
            };

            // Tạo ListBox để hiển thị tên các bài hát trong danh sách yêu thích
            ListBox listBoxFavorites = new ListBox
            {
                Dock = DockStyle.Left,
                Width = 300
            };

            // Tạo FlowLayoutPanel để chứa các nút "Xóa" tương ứng với từng bài hát
            FlowLayoutPanel buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                Width = 80,
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true
            };

            // Duyệt qua danh sách yêu thích, hiển thị tên bài hát và tạo nút "Xóa" cho mỗi bài
            Node current = favoriteSongs.Head;
            while (current != null)
            {
                string songName = Path.GetFileName(current.Data);
                listBoxFavorites.Items.Add(songName);

                // Tạo nút "Xóa" cho bài hát
                Button deleteButton = new Button
                {
                    Text = "Xóa",
                    Tag = current.Data,
                    Width = 60,
                    Height = 25
                };

                // Xử lý sự kiện khi nhấn nút "Xóa": xóa bài hát khỏi danh sách và cập nhật giao diện
                deleteButton.Click += (s, ev) =>
                {
                    string songPath = (string)((Button)s).Tag;
                    favoriteSongs.Remove(songPath); // Xóa bài hát khỏi danh sách yêu thích

                    // Cập nhật giao diện: xóa tên bài hát và nút "Xóa" khỏi giao diện
                    listBoxFavorites.Items.Remove(Path.GetFileName(songPath));
                    buttonPanel.Controls.Remove((Button)s);
                    MessageBox.Show("Đã xóa bài hát khỏi danh sách yêu thích!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                };

                // Thêm nút "Xóa" vào panel
                buttonPanel.Controls.Add(deleteButton);
                current = current.Next;
            }

            // Xử lý sự kiện double-click: phát bài hát khi người dùng double-click vào tên bài hát
            listBoxFavorites.MouseDoubleClick += (s, ev) =>
            {
                if (listBoxFavorites.SelectedItem != null)
                {
                    string selectedSongName = listBoxFavorites.SelectedItem.ToString();
                    Node temp = favoriteSongs.Head;
                    while (temp != null)
                    {
                        if (Path.GetFileName(temp.Data) == selectedSongName)
                        {
                            // Tìm bài hát trong danh sách chính và cập nhật chỉ số
                            currentSong = songList.Find(temp.Data);
                            currentIndex = 0;
                            Node tempSong = songList.Head;
                            while (tempSong != null && tempSong.Data != temp.Data)
                            {
                                currentIndex++;
                                tempSong = tempSong.Next;
                            }

                            // Phát bài hát và đóng form
                            PlayCurrentSong();
                            favoritesForm.Close();
                            break;
                        }
                        temp = temp.Next;
                    }
                }
            };

            // Thêm ListBox và panel vào form, sau đó hiển thị form
            favoritesForm.Controls.Add(listBoxFavorites);
            favoritesForm.Controls.Add(buttonPanel);
            favoritesForm.ShowDialog();
        }

        // Xử lý khi chọn bài hát từ ListBox chính
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                currentSong = songList.GetNodeAt(listBox1.SelectedIndex);
                currentIndex = listBox1.SelectedIndex;
                PlayCurrentSong();
            }

        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentSong != null && currentSong.Next != null)
            {
                currentSong = currentSong.Next;
                currentIndex++;
                PlayCurrentSong();
            }
            else if (songList.Head != null)
            {
                currentSong = songList.Head;
                currentIndex = 0;
                PlayCurrentSong();
            }
            else
            {
                MessageBox.Show("Không còn bài hát tiếp theo!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (songList.Count == 0 || currentSong == null) return;

            if (currentIndex > 0)
            {
                currentIndex--;
                currentSong = songList.GetNodeAt(currentIndex);
                PlayCurrentSong();
            }
            else if (songList.Count > 0)
            {
                currentIndex = songList.Count - 1;
                currentSong = songList.GetNodeAt(currentIndex);
                PlayCurrentSong();
            }
            else
            {
                MessageBox.Show("Đã ở bài hát đầu tiên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnMergeSort_Click(object sender, EventArgs e)
        {
            if (songList.Count > 0)
            {
                songList.MergeSort();
                UpdateListBox();
                currentSong = songList.Head;
                currentIndex = 0;
                PlayCurrentSong();
            }

        }
        // Lịch sử các bài hát
        private void btnShowHistory_Click(object sender, EventArgs e)
        {
            if (songHistory.Count == 0)
            {
                MessageBox.Show("Chưa có lịch sử phát nhạc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Form historyForm = new Form
            {
                Text = "Lịch sử phát nhạc",
                Width = 500,
                Height = 300
            };

            DataGridView dataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false
            };

            // Tạo các cột
            dataGridView.Columns.Add("SongName", "Tên bài hát");
            dataGridView.Columns.Add("PlayTime", "Thời gian phát");
            DataGridViewButtonColumn deleteColumn = new DataGridViewButtonColumn
            {
                Name = "Delete",
                HeaderText = "Xóa",
                Text = "Xóa",
                UseColumnTextForButtonValue = true,
                Width = 60
            };
            dataGridView.Columns.Add(deleteColumn);

            // Thêm dữ liệu vào DataGridView
            Node current = songHistory.Head;
            while (current != null)
            {
                string[] parts = current.Data.Split('|');
                if (parts.Length == 2)
                {
                    string songPath = parts[0];
                    string songName = Path.GetFileName(songPath);
                    string playTime = parts[1];
                    dataGridView.Rows.Add(songName, playTime, current.Data);
                }
                current = current.Next;
            }

            // Xử lý sự kiện click vào nút "Xóa"
            dataGridView.CellClick += (s, ev) =>
            {
                if (ev.ColumnIndex == dataGridView.Columns["Delete"].Index && ev.RowIndex >= 0)
                {
                    string historyEntry = (string)dataGridView.Rows[ev.RowIndex].Cells[2].Value;
                    string songName = (string)dataGridView.Rows[ev.RowIndex].Cells[0].Value;
                    songHistory.Remove(historyEntry);
                    dataGridView.Rows.RemoveAt(ev.RowIndex);
                    MessageBox.Show($"Đã xóa {songName} khỏi lịch sử!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            // Xử lý double-click để phát lại
            dataGridView.CellDoubleClick += (s, ev) =>
            {
                if (ev.RowIndex >= 0 && ev.ColumnIndex != dataGridView.Columns["Delete"].Index)
                {
                    string songPath = ((string)dataGridView.Rows[ev.RowIndex].Cells[2].Value).Split('|')[0];
                    currentSong = songList.Find(songPath);
                    currentIndex = 0;
                    Node tempSong = songList.Head;
                    while (tempSong != null && tempSong.Data != songPath)
                    {
                        currentIndex++;
                        tempSong = tempSong.Next;
                    }
                    PlayCurrentSong();
                    historyForm.Close();
                }
            };

            historyForm.Controls.Add(dataGridView);
            historyForm.ShowDialog();
        }
    }
}
