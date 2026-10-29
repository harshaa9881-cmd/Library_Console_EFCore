using System;
using System.Collections.Generic;
using System.Text;


using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;

namespace LibraryManagementSystem.Menus
{
    public class MemberMenu
    {
        private readonly MemberService _memberService;

        public MemberMenu(MemberService memberService)
        {
            _memberService = memberService;
        }

        public async Task ShowMenuAsync()
        {
            while (true)
            {
                Console.WriteLine("\n--- Member Menu ---");
                Console.WriteLine("1. Add Member");
                Console.WriteLine("2. View All Members");
                Console.WriteLine("3. View Member By ID");
                Console.WriteLine("4. Update Member");
                Console.WriteLine("5. Delete Member");
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine() ?? "";

                try
                {
                    switch (choice)
                    {
                        case "1":
                            await AddMemberAsync();
                            break;

                        case "2":
                            await ViewAllMembersAsync();
                            break;

                        case "3":
                            await ViewMemberByIdAsync();
                            break;

                        case "4":
                            await UpdateMemberAsync();
                            break;

                        case "5":
                            await DeleteMemberAsync();
                            break;

                        case "0":
                            return;

                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Operation failed: {ex.Message}");
                }
            }
        }

        private async Task AddMemberAsync()
        {
            var member = new Member();

            Console.Write("Enter member name: ");
            member.MemberName = Console.ReadLine() ?? "";

            Console.Write("Enter email: ");
            member.Email = Console.ReadLine() ?? "";

            Console.Write("Enter phone number: ");
            member.PhoneNumber = Console.ReadLine() ?? "";

            await _memberService.AddAsync(member);

            Console.WriteLine("Member added successfully.");
        }

        private async Task ViewAllMembersAsync()
        {
            var members = await _memberService.GetAllAsync();

            if (members.Count == 0)
            {
                Console.WriteLine("No members found.");
                return;
            }

            Console.WriteLine(
                "\n{0,-10} {1,-25} {2,-30} {3,-15}",
                "ID", "Name", "Email", "Phone");

            foreach (var member in members)
            {
                Console.WriteLine(
                    "{0,-10} {1,-25} {2,-30} {3,-15}",
                    member.MemberId,
                    member.MemberName,
                    member.Email,
                    member.PhoneNumber);
            }
        }

        private async Task ViewMemberByIdAsync()
        {
            int memberId = ReadInt("Enter member ID: ");

            var member = await _memberService.GetByIdAsync(memberId);

            if (member == null)
            {
                Console.WriteLine("Member not found.");
                return;
            }

            DisplayMember(member);
        }

        private async Task UpdateMemberAsync()
        {
            int memberId = ReadInt("Enter member ID to update: ");

            var member = await _memberService.GetByIdAsync(memberId);

            if (member == null)
            {
                Console.WriteLine("Member not found.");
                return;
            }

            Console.Write("Enter new member name: ");
            member.MemberName = Console.ReadLine() ?? "";

            Console.Write("Enter new email: ");
            member.Email = Console.ReadLine() ?? "";

            Console.Write("Enter new phone number: ");
            member.PhoneNumber = Console.ReadLine() ?? "";

            bool updated = await _memberService.UpdateAsync(member);

            Console.WriteLine(updated
                ? "Member updated successfully."
                : "Member could not be updated.");
        }

        private async Task DeleteMemberAsync()
        {
            int memberId = ReadInt("Enter member ID to delete: ");

            bool deleted = await _memberService.DeleteAsync(memberId);

            Console.WriteLine(deleted
                ? "Member deleted successfully."
                : "Member not found or has issue history. The member cannot be deleted.");
        }

        private void DisplayMember(Member member)
        {
            Console.WriteLine($"Member ID: {member.MemberId}");
            Console.WriteLine($"Name: {member.MemberName}");
            Console.WriteLine($"Email: {member.Email}");
            Console.WriteLine($"Phone: {member.PhoneNumber}");
        }

        private int ReadInt(string message)
        {
            while (true)
            {
                Console.Write(message);

                if (int.TryParse(Console.ReadLine(), out int value))
                {
                    return value;
                }

                Console.WriteLine("Please enter a valid number.");
            }
        }
    }
}
