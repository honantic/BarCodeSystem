   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5       BarCodeSystem'   !BarCodeSystem.BadProductList_Page   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      X TXceed.Wpf.Toolkit, Version=2.4.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f47toolkit)http://schemas.xceed.com/wpf/xaml/toolkit  5      0sys&clr-namespace:System;assembly=mscorlib  5      -   5          N� Title$  不良品明细表��5
       N� ShowsNavigationUI .  .��5      � -   5        $��
fatherGrid��5       � RowDefinitions 5   
   �� 5      $��30 �6   6*   �� 5      $��2* �6   6*   �� 5      $��15 �6   6*   �� 5      $��4* �6   6*   �� 5      $��* �6   6)    � ColumnDefinitions 5   
   �� 5      $��* �6   6+   �� 5      $��10 �6   6,   �� 5      $
��3.5* �6   6.   .+�5   
   �� $��1��6   $��Center=�6'   $��Stretch=�6D   $��5��6`   $��1��6j   $��3��6w   $��0 10 0 0q�6�   ��5      �� 5      .��5      �  � Color$ 	LightBlue��6#    � Offset$	 0.8Q�65   6D   � 5      $ Salmon��6#   $	 0.4Q�62   6A   � 5       $ 
LightGreen��6#   68   5!      5$      �� 5'   
   $��3��6   .��5(      �� $��15(�6   
������� 不良品明细表查询页面6E   ��5)      �� 5*      ��5,      � $ 1Q�6(   $ 
LightGreen��63   6H   � 5-      $ Green��6'   67   5/      51      �� 52   
   $��2��6   $��5��6$   $
��5 10q�6/   
�������$��Center=�6U   $��Stretch=�6p   6�   � 53   
   $��1��6   .+�54      �� -   5        $��	gb_header��54      ��55      �� 56      $$��不良品明细表查询条件��6       textb_NormalStyle#��[  62   6_   .��59      � -   5        $��BadProductHeadGrid��59       5:      �� 5;      6*   �� 5<      6*   �� 5=      6*   �� 5>      6*    5A      �� 5B      6-   �� 5C      $��2* �6+   68   .+�5G      �� $��选择车间:��6    $��0 0 5 0q�6-   $��Center=�6>   $��Right=�6Y   $��0��6u   $��0��6�   6�   �� 5H      $��0��6!   $��1��6.   $
��Left=�6>   $��5q�6Y    �� Orientation$ 
Horizontal=�6�   $��Center=�6�   $��30��6�   ��6d   ��  �� Path$ ActualWidth� �� ElementName$ comobox_year��.+�5I      �� -   5        $��txtb_DeptInfo��5I   `   
�������$��Center=�69   $	��140��6T   	 �� 
IsReadOnly	 . $��点击放大镜选择��6�   
�������6�   �� 5K      -   5        $��btn_DeptSearch��5L   !   
 � Cursor$

 Hand��5K   !     btn_SearchStyle#��[ 6/   $��30��6Y   
�������
���������6x   �� $ ActualHeight�$ txtb_DeptInfo��.��5M      �� $��15��6$   $��/Images/Zoom.ico��60   6L   5N      5O      �� 5Q      $��起始日期：��6    $��1��6-   $��0��6:   $��0 0 5 0q�6J   $��Center=�6[   $��Right=�6v   6�   (  "System.Windows.Controls.DatePicker  5R      -   5        $��datepicker_StartDate��5R   "   $	��180��6?   $

 Hand��6K   $��1��6Y   $��1��6i   $
��Left=�6v   $��Center=�6�   6�   �� 5T      $��结束日期：��6    $��2��6-   $��0��6:   $��0 0 5 0q�6J   $��Center=�6[   $��Right=�6v   6�     5U      -   5        $��datepicker_EndDate��5U   "   
 �� Now   SelectedDate# Z 6<   $	��180��6g   $

 Hand��6s   $��1��6�   $��2��6�   $
��Left=�6�   $��Center=�6�   6�   �� 5W      $��料品型号：��6    $��3��6-   $��0��6:   $��0 0 5 0q�6J   $��Center=�6[   $��Right=�6v   6�   (  "Xceed.Wpf.Toolkit.WatermarkTextBox  5X      -	   5        $��txtb_SearchKey��5X   /   $��Center=�6E   $��14(�6�   $��Center=�6�   $��3��6�   $��1��6�   $
��Left=�6�   $	��180��6�   6   5Y      5Z      5[      � 5]   
   $��3��6   .+�5^      �� $��5q�6   ��5_      �� 5`      #��[  6!   车间信息6S   .��5c      � -
   5        $��DeptGrid��5c       5d      �� 5e      -   5         �Name$ row_top��5e   (   69   �� 5f      $��30 �6(   66   .+�5i      &   System.Windows.Controls.DataGrid  -   5        $��datagrid_Dept��5j   e      ClipboardCopyMode$
 None=�5i         CanUserDeleteRows .     CanUserReorderColumns .     CanUserSortColumns .     CanUserAddRows .     AutoGenerateColumns .     ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 5j   R      RowHeaderWidth$ 30Q�5k         Columns 5l      0  *System.Windows.Controls.DataGridTextColumn  5m      ,  &System.Windows.Controls.DataGridColumn   Header$ 车间编码��61      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6Q   �� $ department_code�6|     5n      $ 车间名称��61    .  6Q   �� $ department_name�6|     5o      $ 简称��61    .  6P   �� $ department_shortname�6�   5q      � 5s      $��1��6    5t      �� 5u      61   �� 5v      61   �� 5w      61   .+�5z      �� -   5        $��	btn_Chose��5z   L   $��选择��6!   $	��120��6.   $��80��6=   $��0��6]   $��Center=�6n   $
��Left=�6�   $��80��6�   6�   �� 5{      -   5        $��	btn_Query��5{   L   $��查询��6!   $	��120��6.   $��80��6=   $��2��6]   $��Center=�6n   $��Right=�6�   $��80��6�   6�   5|      5~      5      5�      �� 5�   
   $��2��6   $��1��6#   $��3��60   ��5�      �� 5�      $��不良品明细表��6   #��[  6*   6W   .��5�       � -   5        $��frame_Search��5�        � JournalOwnership$ OwnsJournal=�6(     � NavigationUIVisibility$ Hidden=�6G   $��10q�5�      $��2��6   $��1��6,   $��3��69   6L   5�      5�      5�      