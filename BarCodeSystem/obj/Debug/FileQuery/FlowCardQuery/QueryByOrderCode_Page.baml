   M S B A M L   `   `   `  ����  BarCodeSystemA    ;BarCodeSystem.FileQuery.FlowCardQuery.QueryByOrderCode_Page   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      X TXceed.Wpf.Toolkit, Version=2.4.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f46extool)http://schemas.xceed.com/wpf/xaml/toolkit  5      -   5          N� Title$  QueryByOrderCode_Page��5
      .��5      �  � RowDefinitions 5   
   �� 5      $��30 �6   6*   �� 5      $��40 �6   6*   �� 5      $��40 �6   6*   �� 5      6   �� 5      $��40 �6   6*   .+�5   
   �� $!��当前按生产订单查询��6   $��Center=�6%   $��Center=�6@   6_   � 5   
   $��2��6    � ColumnDefinitions 5      �� 5      6%   �� 5      $��15 �6#   60   .+�5      (  "Xceed.Wpf.Toolkit.WatermarkTextBox  -   5          Name$ txtb_SourceOrderCode��5   4   $��25��6(   $��5 0 0 0q�5   "      	Watermark$ 回车搜索生产订单��63    �� TextWrapping$ WrapWithOverflow=�6I   .��6i   5      �� 5      -   5        $��btn_FlowCardSearch��5   !   $��25��6   $��1��6<    � Cursor$
 Hand��5         btn_SearchStyle#��[  6D   
�������
�������.��5      �� $��15��6   $��../../Images/Zoom.ico��6$   6E   5       5!      �� 5%   
   -   5        $��btn_Back��5%   �   $��1��6   $��5q�6   $��25��6)   $
 Hand��67     bd_SelectStyle#��[ 6E   $��0q�6m   $	��125��6�   ��5&      �� 5'      -   5        $��text_Content��5'      62   5)      �� 5+   
   $��3��6   ��5,      �� 5-        textb_NormalStyle#��[ 6   流转卡列表6O   .��5/      &   System.Windows.Controls.DataGrid  -   5        $��datagrid_FlowCardList��5/   �      
IsReadOnly .    RowHeaderWidth$ 30Q�6�   	   AutoGenerateColumns	 .  
   CanUserAddRows
 .     CanUserDeleteRows .     Columns 50      0  *System.Windows.Controls.DataGridTextColumn  51      ,  &System.Windows.Controls.DataGridColumn   Header$ 流转卡编号��6J   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6)   ��  �� Path$ FC_Code�6[   53      54      �� 56   
   -   5        $��
btn_Select��56   R   $��5q�6   $��25��6   $��4��6(   $	��120��65   $��选定��6E   6   58      59      