   M S B A M L   `   `   `  ����  BarCodeSystem>    8BarCodeSystem.ProductDispatch.FlowCard.SaveWorkTeam_Page   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      X TXceed.Wpf.Toolkit, Version=2.4.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f46extool)http://schemas.xceed.com/wpf/xaml/toolkit  5      -   5          N� Title$  SaveWorkTeam_Page��5
      .��5      � .+�5   
   �  � RowDefinitions 5      �� 5      $��30 �6    6.   �� 5      6#   �� 5      $��30 �6    6.   .+�5      �� .��5      �� -   5        $��textb_Header��5      
�������6E   ��5      �� 5      ��5      �  � Offset$ 1Q�6+    � Color$ 	Chocolate��66   6J   � 5      $	 0.5Q�6,   $ LightSalmon��69   6O   � 5      $ Crimson��6+   6=   5      5      &   System.Windows.Controls.DataGrid  5       -   5        $��datagrid_WorkTeamInfo��5    0      ClipboardCopyMode$
 None=�6      
IsReadOnly .    AutoGenerateColumns .     CanUserAddRows .  $��1��5!   .      RowHeaderWidth$ 30Q�6;   	   ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$
	 Auto 6O   ��5    �   �� 
 �� 
GroupStyle
 5"      (  "System.Windows.Controls.GroupStyle  5#         ContainerStyle 5$      �� 5%       �� 
TargetType" ��6$   .�5&   "   ��  �� Property � �� �� Value 5'   &   �� 5(   *    �� 
TargetType" ��6:   .R�5)   .   9�  9� 
IsExpanded . ��5*   2   �� 5+   6   ��6@   ��  �� Path$
 Name�6]   .��5-   2   �� 6C   5.   /   5/   +   51   #   52         Panel 54      �� 55      .R�56   "   >  8System.Windows.Controls.Primitives.DataGridRowsPresenter  57      59         Columns 5;      0  *System.Windows.Controls.DataGridTextColumn  5<      ,  &System.Windows.Controls.DataGridColumn   Header$ 车间��6)      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6J   �� $ WTM_WorkCenterName�6x     5=      $ 班组编号��6)    67   �� $ WTM_WorkTeamCode�6c     5>      $ 班组名称��6)    67   �� $ WTM_WorkTeamName�6c     5?      $ 成员编号��6)    .  6I   �� $ WTM_MemberPersonCode�6y     5@      $ 成员姓名��6)    .  6I   �� $ WTM_MemberPersonName�6y   5B      � 5C      $��2��6    � ColumnDefinitions 5D      �� 5E      6)   �� 5F      6)   �� 5G      6)   .+�5I      �� -   5        $��btn_Refresh��5I      $��1��6,   $��	10 0 10 0q�6V   $��刷新列表��6i   6z   �� 5J      -   5        $��btn_SaveWorkTeam��5J      $��2��61   $
��10 0q�6`   $��保存班组��6n   6   �� 5K      -   5        $��
btn_Submit��5K      $��2��6+   $��	10 0 10 0q�6T   $��选定班组��6g   6x   �� 5L      -   5        $��panel_Search��5L       �� Orientation$ 
Horizontal=�62   $��Center=�6K   $��30��6f   .+�5M      (  "Xceed.Wpf.Toolkit.WatermarkTextBox  -   5        $��txtb_TeamInfo��5M   O      	Watermark$ 搜索班组信息��6.   $	��0 5q�6A   
���������6d   �� $ ActualWidth� �� ElementName$ 
btn_Submit��.��6�   5N      �� 5O      -	   5        $��btn_TeamSearch��5O       � Cursor$
 Hand��63      btn_SearchStyle#��[  6^   
�������
�������.��5P      �� $��15��6    $��../../Images/Zoom.ico��6,   6M   5Q      5R      �� 5S      -
   5        $��panel_AddTeamCode��5S      $ 
Horizontal=�67   $��Center=�6P   $��30��6k   .+�5T        -   5        $��txtb_TeamCodeInfo��5T   Q   $ 批量设置班组编号��6.   $	��0 5q�6C   
���������6j   �� $ ActualWidth�$ 
btn_Submit��.��5U   b   5V      �� 5W      -   5        $��btn_TeamCodeAdd��5W      $
 Hand��64    � ToolTip$0 *检查编号是否存在并且批量设置��5X      #��[  66   
�������
�������.��5Z      �� $��15��6    $��../../Images/Zoom.ico��6,   6M   5[      5\      �� 5^      -   5        $��panel_AddTeamName��5^      $��1��66   $ 
Horizontal=�6G   $��Center=�6`   $��30��6{   .+�5_        -   5        $��txtb_TeamNameInfo��5_   Q   $ 批量设置班组名称��6.   $	��0 5q�6C   
���������6j   �� $ ActualWidth�$ 
btn_Submit��.��5`   b   5a      �� 5b      -   5        $��btn_TeamNameAdd��5b      $
 Hand��64   $ 批量设置名称��5c      #��[  6.   
�������
�������.��5e      �� $��15��6    $��../../Images/Zoom.ico��6,   6M   5f      5g      5h      5i      5j      5k      