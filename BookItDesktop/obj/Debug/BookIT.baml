   M S B A M L   `   `   `  ����H  DBookItDesktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:BookItDesktopBookItDesktop  5   	       BookItDesktop.BookIT   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   \ XHardcodet.Wpf.TaskbarNotification, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null)tb http://www.hardcodet.net/taskbar  5   	   :d2http://schemas.microsoft.com/expression/blend/2008  5   	   Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  5   	   'localclr-namespace:BookItDesktop  5   	   -   5          � Title$  BookIT��5	   	   $	��430��6   $	��804��6%   ���#2D3233.��5      �  � 	Resources 5   
      BookItDesktop.BookIT   TrayBookClick��%!   &	          5      5   	   5      .+�5   
   3  -Hardcodet.Wpf.TaskbarNotification.TaskbarIcon  -   5          Name$ taskBarIconMain��5         ToolTipText$ hello world��5       � ContextMenu 5      �� 5      
�������.@�5      m� -   5         m�Name$ 	iconItem1��5   9   $��Activae interface��6L   m� 5      -   5        $ 	iconItem2��5   8   $
��Quit��6L   5      5"      �� 5$   
   -   5         ��Name$ 
tabControl��5$      $
��Left=�6)   $	��399��6D   $	��Top=�6Q   $	��796��6i   $��0,0,0,0q�6u   ���#202526.@�5%      �� -   5         ��Name$ tabItemList��5%      $��Bookmark List��6+   $	��200��6B   $	��100��6N   ���#2D3233���#D0D9DB.��5&      �� -   5         ��Name$ testView��5&   D   ���#202526
�������$
��Left=�6V   $	��294��6q   $��0,0,-6,0q�6~   $	��Top=�6�   $	��796��6�   	 �� View	 5'      �� 5)      .P�5*      �� 
 �� Width$	
 350��6-    �� TextAlignment$ Center=�6:   $	��Url��6[    �� DisplayMemberBinding 6h   ��  �� Path$
 url1��� 5+      $	
 146��6-   $ Center=�6<   $
��Tags��6]    6k   �� $
 tag1��� 5,      $	
 246��6-   $
��test��69    �� CellTemplate 5-   "   �� 5.   &   .R�5/   *   �� $��EditBookmark��6=   -   5        $	��100��5/   1   $
��Edit��6Q   ��6z   �� $ id�50   '   54      55      57      58      �� 59      -	   5        $ 
tabItemAdd��59      $�� Add Bookmark��6*   $	��200��6A   $	��100��6M   ���#2D3233���#D0D9DB.��5:      � -
   5         �Name$ gridAdd��5:   .   ���#202526$	��294��6@   $	��796��6N   5;      �� 5<      -   5        $ tabItemOptions��5<      $��Options��6.   $	��200��6?   $	��100��6K   ���#2D3233���#D0D9DB.��5=      � ���#202526.+�5>      �� -   5         ��Name$ labelApplicationStartup��5>      $$��Launch application at startup:��6=   ���#D0D9DB6}   �� 5?      -   5         ��Name$% checkBoxAddApplicationToStartUp��5?      $��Center=�6H   $	��Top=�6e   $��20��6}   $��13��6�   $��195,10,582,0q�6�   �� 5@      -   5         ��Name$ btnSaveOptions��5@      $	��150��65   $��50��6A   $��Bottom=�6M   $��Center=�6h   $��0,0,0,0q�6�   $
��Save��6�   
���OO/����#D0D9DB6�   5A      5B      �� 5C      -   5        $ tabItemLogOut��5C      $��Logout��6.   $	��190��6>   $	��100��6J   ���#2D3233���#D0D9DB.��5D      � ���#202526.+�5E      �� -   5        $ 	labelTest��5E      $��Label��6/   $
��Left=�6?   $��
140,82,0,0q�6Z   $	��Top=�6n   5F      5H      5J      5K      5L      