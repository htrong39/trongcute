using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


namespace List_am_nhac
{
   
    public partial class Form1 : Form
    {
        // Các biến toàn cục
        private DoublyLinkedList songList = new DoublyLinkedList();      // Danh sách bài hát chính
        private DoublyLinkedList songHistory = new DoublyLinkedList();   // Lịch sử phát
        private DoublyLinkedList songQueue = new DoublyLinkedList();     // Hàng đợi phát
        private DoublyLinkedList favoriteSongs = new DoublyLinkedList(); // Danh sách yêu thích
        private Node currentSong;                                        // Bài hát hiện tại
        private bool isLooping = false;                                  // Trạng thái lặp
        private int currentIndex = 0;                                    // Vị trí hiện tại
        private Form suggestionForm;                                      // Form gợi ý
        private ListBox listBoxSuggestions;                              // ListBox gợi ý

        // Khởi tạo form
        public Form1()
        {
            InitializeComponent();
            InitializeEventHandlers();

        }
        private void InitializeEventHandlers()
        {
            axWindowsMediaPlayer1.PlayStateChange += axWindowsMediaPlayer1_PlayStateChange;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
        }
        // Lớp Node cho Doubly Linked List

        // --------------------- CÁC LỚP DỮ LIỆU ---------------------

        // Lớp Node cho Doubly Linked List
        public class Node
        {
            public string Data { get; set; }
            public Node Previous { get; set; }
            public Node Next { get; set; }

            public Node(string data)
            {
                Data = data;
                Previous = null;
                Next = null;
            }
        }

        // Lớp DoublyLinkedList tự định nghĩa
        public class DoublyLinkedList
        {
            public Node Head { get; private set; }
            public Node Tail { get; private set; }
            public int Count { get; private set; }

            public DoublyLinkedList()
            {
                Head = null;
                Tail = null;
                Count = 0;
            }

            // Thêm vào đầu (cho lịch sử)
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

            // Thêm vào cuối (cho danh sách và hàng đợi)
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

            // Xóa và trả về phần tử đầu (cho lịch sử và hàng đợi)
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

            // Tìm node theo dữ liệu
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

            // Lấy node tại vị trí index
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

            // Xáo trộn danh sách trực tiếp
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

            // Sắp xếp danh sách bằng MergeSort
            public void MergeSort()
            {
                if (Count <= 1) return; // Nếu danh sách rỗng hoặc chỉ có 1 phần tử thì không cần sắp xếp
                Head = MergeSortList(Head);
                // Cập nhật Tail sau khi sắp xếp
                Tail = Head;
                while (Tail != null && Tail.Next != null)
                {
                    Tail = Tail.Next;
                }
            }

            // Hàm hỗ trợ MergeSort: chia và sắp xếp danh sách
            private Node MergeSortList(Node head)
            {
                // Trường hợp cơ sở: nếu danh sách rỗng hoặc chỉ có một phần tử
                if (head == null || head.Next == null)
                    return head;

                // Chia danh sách thành hai nửa
                Node slow = head;    // Con trỏ chậm
                Node fast = head.Next; // Con trỏ nhanh

                while (fast != null && fast.Next != null)
                {
                    slow = slow.Next;
                    fast = fast.Next.Next;
                }

                Node secondHalf = slow.Next; // Nửa thứ hai của danh sách
                slow.Next = null;
                if (secondHalf != null)
                    secondHalf.Previous = null;

                // Sắp xếp đệ quy hai nửa
                Node left = MergeSortList(head);
                Node right = MergeSortList(secondHalf);

                // Hợp nhất hai nửa đã sắp xếp
                return Merge(left, right);
            }

            // Hàm hỗ trợ MergeSort: hợp nhất hai danh sách đã sắp xếp
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

                // Cập nhật con trỏ Previous cho đầu danh sách đã hợp nhất
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
            }
        }

        // Cập nhật danh sách hiển thị trong ListBox
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
        // Phát bài hát tiếp theo
        private void PlayNextSong()
        {
            if (songList.Count == 0) return;

            int nextIndex = currentIndex + 1;
            if (nextIndex >= songList.Count)
            {
                if (isLooping)
                {
                    nextIndex = 0;
                }
                else
                {
                    return;
                }
            }

            currentIndex = nextIndex;
            currentSong = songList.GetNodeAt(nextIndex);
            PlayCurrentSong();
        }

        // --------------------- SỰ KIỆN MEDIA PLAYER ---------------------

        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (e.newState == (int)WMPLib.WMPPlayState.wmppsMediaEnded)
            {
                MessageBox.Show("Bài hát đã kết thúc, chuyển sang bài tiếp theo...");
                PlayNextSong();
            }
        }
        // --------------------- SỰ KIỆN NÚT BẤM ---------------------
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

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentSong != null && currentSong.Next != null)
            {
                songHistory.AddFirst(currentSong.Data);
                currentSong = currentSong.Next;
                currentIndex++;
                PlayCurrentSong();
            }
            else if (isLooping && songList.Head != null)
            {
                songHistory.AddFirst(currentSong.Data);
                currentSong = songList.Head;
                currentIndex = 0;
                PlayCurrentSong();
            }
            else
            {
                MessageBox.Show("Không còn bài hát tiếp theo!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (songHistory.Count > 0)
            {
                string lastPlayed = songHistory.RemoveFirst();
                currentSong = songList.Find(lastPlayed);
                currentIndex = 0;
                Node temp = songList.Head;
                while (temp != null && temp.Data != lastPlayed)
                {
                    currentIndex++;
                    temp = temp.Next;
                }
                PlayCurrentSong();
            }
            else
            {
                MessageBox.Show("Không có bài hát trước đó!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnMergeSort_Click(object sender, EventArgs e)
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

        // --------------------- SỰ KIỆN MEDIA PLAYER ---------------------
        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {
            if (isLooping && axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsStopped)
            {
                PlayCurrentSong();
            }
        }
        // --------------------- SỰ KIỆN NÚT BẤM ---------------------

        // Bật/tắt chế độ lặp
        private void btnLoop_Click(object sender, EventArgs e)
        {
            isLooping = !isLooping;
            MessageBox.Show(isLooping ? "Lặp bài hát bật" : "Lặp bài hát tắt", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        // Quản lý hàng đợi
        private void btnAddToQueue_Click(object sender, EventArgs e)
        {
            if (currentSong != null)
            {
                songQueue.AddLast(currentSong.Data);
                MessageBox.Show("Đã thêm vào hàng đợi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnClearQueue_Click(object sender, EventArgs e)
        {
            songQueue.Clear();
            MessageBox.Show("Đã xóa hàng đợi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnPlayQueue_Click(object sender, EventArgs e)
        {
            if (songQueue.Count > 0)
            {
                string nextSong = songQueue.RemoveFirst();
                currentSong = songList.Find(nextSong);
                currentIndex = 0;
                Node temp = songList.Head;
                while (temp != null && temp.Data != nextSong)
                {
                    currentIndex++;
                    temp = temp.Next;
                }
                PlayCurrentSong();
            }
            else
            {
                MessageBox.Show("Hàng đợi trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            else if (isLooping && songList.Count > 0)
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
        //Tìm kiếm và gợi ý
        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            Form inputForm = new Form
            {
                Text = "Chọn ký tự đầu của bài hát",
                Size = new Size(400, 200)
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
                Size = new Size(300, 400)
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
        private void listBoxSuggestions_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBoxSuggestions.SelectedItem != null)
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
        }
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

        private void btnShowFavorites_Click(object sender, EventArgs e)
        {
            if (favoriteSongs.Count == 0)
            {
                MessageBox.Show("Danh sách yêu thích trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Form favoritesForm = new Form
            {
                Text = "Danh sách yêu thích",
                Size = new Size(200, 200)
            };

            ListBox listBoxFavorites = new ListBox { Dock = DockStyle.Fill };
            Node current = favoriteSongs.Head;
            while (current != null)
            {
                listBoxFavorites.Items.Add(Path.GetFileName(current.Data));
                current = current.Next;
            }

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
                            currentSong = songList.Find(temp.Data);
                            currentIndex = 0;
                            Node tempSong = songList.Head;
                            while (tempSong != null && tempSong.Data != temp.Data)
                            {
                                currentIndex++;
                                tempSong = tempSong.Next;
                            }
                            PlayCurrentSong();
                            favoritesForm.Close();
                            break;
                        }
                        temp = temp.Next;
                    }
                }
            };

            favoritesForm.Controls.Add(listBoxFavorites);
            favoritesForm.ShowDialog();
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                currentSong = songList.GetNodeAt(listBox1.SelectedIndex);
                currentIndex = listBox1.SelectedIndex;
                PlayCurrentSong();
            }
        }
        // danh sách chờ 
        private void button1_Click(object sender, EventArgs e)
        {
            if (songQueue.Count == 0)
            {
                MessageBox.Show("Danh sách chờ trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Form queueForm = new Form
            {
                Text = "Danh sách chờ",
                Size = new Size(400, 400)
            };

            ListBox listBoxQueue = new ListBox { Dock = DockStyle.Fill };
            Node current = songQueue.Head;
            while (current != null)
            {
                listBoxQueue.Items.Add(Path.GetFileName(current.Data));
                current = current.Next;
            }

            listBoxQueue.MouseDoubleClick += (s, ev) =>
            {
                if (listBoxQueue.SelectedItem != null)
                {
                    string selectedSongName = listBoxQueue.SelectedItem.ToString();
                    Node temp = songQueue.Head;
                    while (temp != null)
                    {
                        if (Path.GetFileName(temp.Data) == selectedSongName)
                        {
                            currentSong = songList.Find(temp.Data);
                            currentIndex = 0;
                            Node tempSong = songList.Head;
                            while (tempSong != null && tempSong.Data != temp.Data)
                            {
                                currentIndex++;
                                tempSong = tempSong.Next;
                            }
                            PlayCurrentSong();
                            queueForm.Close();
                            break;
                        }
                        temp = temp.Next;
                    }
                }
            };

            queueForm.Controls.Add(listBoxQueue);
            queueForm.ShowDialog();
        }
    }
}

