   M S B A M L   `   `   `  ����J  FFolderExplorer+, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:FolderExplorer_FolderExplorer_  5   	        FolderExplorer_.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:FolderExplorer_     5   	     � Title$  
MainWindow��5   	   $	��650��6   $
��1250��6)   ���	#FF2A2A2E.��5	      �  � ColumnDefinitions 5
   
   �� 5      $
��auto �6   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $��20 �6   �� 5      $��* �6   �� 5      $��20 �6   �� 5      $��20 �6   .+�5   
   �� -   5         ��Name$ 
BackButton��5      $��20��6%   $��<��60   �� 5   
   -   5         ��Name$ Input��5      $��1��6!   $��0��61   
���������6V   Z� 6l   �� 5   
   $��1��6   $��2��6$   .��5      �� -   5         ��Name$ Folders��5      5      5      �� 56   
   -   5         ��Name$ Status��56      $��2��6$   $��2��61   
�������6E   �� 57   
   -   5         ��Name$ 
SortOption��57      $��0��6'   $��3��67   $��2��6D   
���   � �� SelectedIndex$ 1��6�   ��6X   Z� ��6�   Z� .@�58      �� $��Alphabetical��6   �� 59      $��↓ Size ↓��6   �� 5:      $��↑ Size ↑��6   5;      5=      5>      